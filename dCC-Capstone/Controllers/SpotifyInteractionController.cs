
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Capstone.Models;
using Newtonsoft.Json;
using Microsoft.AspNet.Identity;
using System.Net.Http.Headers;
using Capstone.JsonTemplateClasses;

namespace Capstone.Controllers
{
    public class SpotifyInteractionController : Controller
    {
        private static ApplicationDbContext db = new ApplicationDbContext();
        private static HttpClient httpClient = new HttpClient();
        private static string storedState;
        private const string redirect_url = "https://localhost:44353/Listeners/AuthResponse";

        public static Uri GetSpotifyAuthorization()
        {
            storedState = Randomness.RandomString();
            var url = $"https://accounts.spotify.com/authorize?client_id={Keys.SpotifyClientId}&response_type=code&redirect_uri={redirect_url}&scope=playlist-modify-private,user-read-private&state={storedState}";
            Uri uri = new Uri(url);
            return uri;
        }

        public async static Task<SpotifyAuthorizationTokenJsonResponse.Rootobject> PostSpotifyOauthToReceiveSpotifyAuthAndRefreshToken(string code, string state, Listener listener)
        {
            if (state != storedState)
            {
                throw new Exception("Error: state returned from Spotify is not correct");
            }
            string url = "https://accounts.spotify.com/api/token";

            //httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var parameters = new Dictionary<string, string>();
            parameters.Add("grant_type", "authorization_code");
            parameters.Add("code", code);
            parameters.Add("redirect_uri", redirect_url);
            var content = await SendSpotifyHttpRequest(url, "POST", listener, parameters);
            var jsonResponse = await content.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeObject<SpotifyAuthorizationTokenJsonResponse.Rootobject>(jsonResponse);
            return token;
        }

        public async static Task<SpotifyRefreshTokenJsonResponse.Rootobject> GetNewSpotifyAccessToken(Listener listener)
        {
            string url = "https://accounts.spotify.com/api/token";
            var parameters = new Dictionary<string, string>();
            parameters.Add("grant_type", "refresh_token");
            parameters.Add("refresh_token", listener.RefreshToken);
            var response = await SendSpotifyHttpRequest(url, "POST", listener, parameters);
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeObject<SpotifyRefreshTokenJsonResponse.Rootobject>(jsonResponse);
            return token;
        }

        public async static Task<Artist> SpotifySearchForArtistInGenre(Genre genre, Listener listener)
        {
            if (genre is null || listener is null)
            {
                return null;
            }
            string offset = Randomness.RandomInt(0, 100).ToString();
            string url = $"https://api.spotify.com/v1/search?q=genre:{genre.GenreSpotifyName}&type=artist&offset={offset}&limit=1";

            var response = await SendSpotifyHttpRequest(url, "GET", listener);
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var artistsRootobject = JsonConvert.DeserializeObject<SpotifyArtistsSearchJsonResponse.Rootobject>(jsonResponse);
            //try
            //{
            if (artistsRootobject.artists.items.Length > 0)
            {
                var artistItem = artistsRootobject.artists.items[0];
                string songPreviewUrl = await GetArtistTopTrackPreviewUrl(listener, artistItem.id);
                List<Genre> artistGenres = new List<Genre>();
                for (int i = 0; i < artistItem.genres.Length; i++)
                {
                    var artistGenre = new Genre() { GenreSpotifyName = artistItem.genres[i] };
                    var artistGenreString = artistItem.genres[i];
                    var genreInDb = db.Genres.FirstOrDefault(g => g.GenreSpotifyName == artistGenreString);
                    if (genreInDb is null)
                    {
                        genreInDb = db.Genres.Add(artistGenre);
                        db.SaveChanges();
                    }
                    artistGenres.Add(genreInDb);
                }

                if (artistItem.images.Length > 0)
                {
                    return new Artist()
                    {
                        ArtistName = artistItem.name,
                        ArtistSpotifyId = artistItem.id,
                        ArtistPopularity = artistItem.popularity,
                        ArtistGenres = artistGenres,
                        ArtistImageUrl = artistItem.images[0].url,
                        ArtistSpotifyUrl = artistItem.external_urls.spotify,
                        ArtistTopTrackPreviewUrl = songPreviewUrl,
                        SearchedGenre = genre.GenreSpotifyName
                    };
                }
            }
            return null;
            //}
            //catch (Exception)
            //{
            //    return null;
            //}
        }
        public async static Task<string> GetArtistTopTrackPreviewUrl(Listener listener, string artistSpotifyId)
        {
            string url = $"https://api.spotify.com/v1/artists/{artistSpotifyId}/top-tracks?country=US";
            var content = await SendSpotifyHttpRequest(url, "GET", listener);
            var jsonResponse = await content.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<SpotifyArtistTopTracksJsonResponse.Rootobject>(jsonResponse).tracks[0].preview_url;
        }


        public async static Task<Object> GetTrackDetailsFromSpotify(Listener listener, string trackSpotifyId)
        {
            string url = "https://api.spotify.com/v1/";
            var content = await SendSpotifyHttpRequest(url, "GET", listener);
            var jsonResponse = await content.Content.ReadAsStringAsync();
            var genreStrings = JsonConvert.DeserializeObject<SpotifyGenresJsonResponse.Rootobject>(jsonResponse).genres.ToList();
            if (!db.Genres.Any())
            {
                foreach (var genre in genreStrings)
                {
                    Genre newGenre = new Genre { GenreSpotifyName = genre };
                    db.Genres.Add(newGenre);
                }
                db.SaveChanges();
            }
            var genres = new List<Genre>(db.Genres.ToList());
            return genres;
        }

        public async static Task<Playlist> SpotifySearchForRecommendedTracks(Listener listener, Playlist playlist)
        {
            if (listener is null)
            {
                throw new Exception("Error: Authenticated listener is required to make API call.");
            }
            string trackLimitNumber = "5";
            StringBuilder urlBuilder = new StringBuilder($"https://api.spotify.com/v1/recommendations");
            urlBuilder.Append("?limit=" + trackLimitNumber);
            urlBuilder.Append("&target_popularity=" + playlist.PopularityTarget);
            urlBuilder.Append("&market=US");

            //if (listener.ListenerGenres.Count > 0)
            //{
                urlBuilder.Append("&seed_genres=");
            //    bool prependComma = false;
            //    for (int i = 0; i < 5; i++)
            //    {
                    int randomIndex = Randomness.RandomInt(0, listener.ListenerGenres.Count);
            //        if (prependComma)
            //        {
            //            urlBuilder.Append(",");
            //        }
                    urlBuilder.Append(listener.ListenerGenres[randomIndex].GenreSpotifyName);
            //        prependComma = true;
            //    }
            //}

            //else if (listener.ListenerArtists.Count > 0)
            //{
            //    urlBuilder.Append("&seed_artists=");
            //    bool prependComma = false;
            //    for (int i = 0; i < 5; i++)
            //    {
            //        int randomIndex = Randomness.RandomInt(0, listener.ListenerArtists.Count);
            //        if (prependComma)
            //        {
            //            urlBuilder.Append(",");
            //        }
            //            urlBuilder.Append(listener.ListenerArtists[randomIndex].ArtistSpotifyId);
            //        prependComma = true;
            //    }
            //}

            //else if (listener.ListenerTracks.Count > 0)
            //{
            //    urlBuilder.Append("&seed_tracks=");
            //    bool prependComma = false;
            //    for (int i = 0; i < 5; i++)
            //    {
            //        int randomIndex = Randomness.RandomInt(0, listener.ListenerTracks.Count);
            //        if (prependComma)
            //        {
            //            urlBuilder.Append(",");
            //        }
            //        urlBuilder.Append(listener.ListenerTracks[randomIndex].TrackSpotifyId);
            //        prependComma = true;
            //    }
            //}

            //else
            //{
            //    Console.WriteLine("Error, we need to have a liked Artist, Genre, or Track to generate a playlist");
            //    return null;
            //}


            if (playlist.Mood != null)
            {
                //urlBuilder.Append("&min_valence=" + playlist.Mood.MoodValenceMinimum.ToString());
                //urlBuilder.Append("&max_valence=" + playlist.Mood.MoodValenceMaximum.ToString());
                urlBuilder.Append("&target_valence=" + playlist.Mood.MoodValenceTarget.ToString());
                //urlBuilder.Append("&min_tempo=" + playlist.Mood.MoodTempoMinimum.ToString());
                //urlBuilder.Append("&max_tempo=" + playlist.Mood.MoodTempoMaximum.ToString());
                urlBuilder.Append("&target_tempo=" + playlist.Mood.MoodTempoTarget.ToString());
                //urlBuilder.Append("&min_energy=" + playlist.Mood.MoodEnergyMinimum.ToString());
                //urlBuilder.Append("&max_energy=" + playlist.Mood.MoodEnergyMaximum.ToString());
                urlBuilder.Append("&target_energy=" + playlist.Mood.MoodEnergyTarget.ToString());
                //urlBuilder.Append("&min_danceability=" + playlist.Mood.MoodDanceabilityMinimum.ToString());
                //urlBuilder.Append("&max_danceability=" + playlist.Mood.MoodDanceabilityMaximum.ToString());
                urlBuilder.Append("&target_danceability=" + playlist.Mood.MoodDanceabilityTarget.ToString());
                //urlBuilder.Append("&min_acousticness=" + playlist.Mood.MoodAcousticnessMinimum.ToString());
                //urlBuilder.Append("&max_acousticness=" + playlist.Mood.MoodAcousticnessMaximum.ToString());
                urlBuilder.Append("&target_acousticness=" + playlist.Mood.MoodAcousticnessTarget.ToString());
                //urlBuilder.Append("&min_speechiness=" + playlist.Mood.MoodSpeechinessMinimum.ToString());
                //urlBuilder.Append("&max_speechiness=" + playlist.Mood.MoodSpeechinessMaximum.ToString());
                urlBuilder.Append("&target_speechiness=" + playlist.Mood.MoodSpeechinessTarget.ToString());
                //urlBuilder.Append("&min_instrumentalness=" + playlist.Mood.MoodInstrumentalnessMinimum.ToString());
                //urlBuilder.Append("&max_instrumentalness=" + playlist.Mood.MoodInstrumentalnessMaximum.ToString());
                urlBuilder.Append("&target_instrumentalness=" + playlist.Mood.MoodInstrumentalnessTarget.ToString());
                //urlBuilder.Append("&min_liveness=" + playlist.Mood.MoodLivenessMinimum.ToString());
                //urlBuilder.Append("&max_liveness=" + playlist.Mood.MoodLivenessMaximum.ToString());
                urlBuilder.Append("&target_liveness=" + playlist.Mood.MoodLivenessTarget.ToString());
                //urlBuilder.Append("&min_loudness=" + playlist.Mood.MoodLoudnessMinimum.ToString());
                //urlBuilder.Append("&max_loudness=" + playlist.Mood.MoodLoudnessMaximum.ToString());
                urlBuilder.Append("&target_mode=" + playlist.Mood.IsInMajorKeyMood.ToString());
            }
            if (playlist.DynamicTracksOnly)
            {
                urlBuilder.Append("&max_loudness=-16");
            }

            string url = urlBuilder.ToString();
            var response = await SendSpotifyHttpRequest(url, "GET", listener);
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var recommendedTracks = JsonConvert.DeserializeObject<SpotifyRecommendedTracksJsonResponse.Rootobject>(jsonResponse);
            List<Track> playlistTracks = new List<Track>();
            foreach (var track in recommendedTracks.tracks)
            {
                Track newTrack = new Track()
                {
                    TrackAlbumSpotifyId = track.album.id,
                    TrackSpotifyId = track.id,
                    TrackName = track.name,
                    TrackPopularity = track.popularity,
                    TrackSpotifyUrl = track.external_urls.spotify
                };

                var trackArtist = track.artists[0];
                Artist artistInDb = db.Artists.FirstOrDefault(a => a.ArtistSpotifyId == trackArtist.id);

                if (artistInDb is null)
                {
                    artistInDb = new Artist()
                    {
                        ArtistName = trackArtist.name,
                        ArtistSpotifyId = trackArtist.id,
                        ArtistSpotifyUrl = trackArtist.external_urls.spotify
                    };
                    artistInDb = db.Artists.Add(artistInDb);
                    db.SaveChanges();
                }

                newTrack.TrackArtistId = artistInDb.ArtistId;
                Track newTrackInDb = db.Tracks.FirstOrDefault(t => t.TrackSpotifyId == track.id);
                if (newTrackInDb is null)
                {
                    newTrackInDb = db.Tracks.Add(newTrack);
                    db.SaveChanges();
                }
                playlistTracks.Add(newTrackInDb);
            }
            playlist.PlaylistTracks = playlistTracks;

            return playlist;
        }

        public async static Task<List<Genre>> SpotifyGenerateGenreSeeds(Listener listener)
        {
            //GET https://api.spotify.com/v1/search
            //Authorization: Bearer {access token}

            string url = "https://api.spotify.com/v1/recommendations/available-genre-seeds";
            var content = await SendSpotifyHttpRequest(url, "GET", listener);
            var jsonResponse = await content.Content.ReadAsStringAsync();
            var genreStrings = JsonConvert.DeserializeObject<SpotifyGenresJsonResponse.Rootobject>(jsonResponse).genres.ToList();
            if (!db.Genres.AsNoTracking().Any())
            {
                foreach (var genre in genreStrings)
                {
                    Genre newGenre = new Genre { GenreSpotifyName = genre };
                    db.Genres.Add(newGenre);
                }
                db.SaveChanges();
            }
            var genres = new List<Genre>(db.Genres.ToList());
            return genres;
        }

        //public async static Task<SpotifyTrackAudioFeaturesJsonResponse> GetSpotifyTrackDetails()
        public async static Task<HttpResponseMessage> SendSpotifyHttpRequest(string url, string type, Listener listener, Dictionary<string, string> postParameters = null)
        {
            Uri uri = new Uri(url);
            HttpResponseMessage response;
            switch (type)
            {
                case "GET":
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", listener.AccessToken);
                    response = await httpClient.GetAsync(uri);
                    break;
                case "POST":
                    var combinedId = Convert.ToBase64String(Encoding.UTF8.GetBytes(Keys.SpotifyClientId + ":" + Keys.SpotifyClientSecret));
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", combinedId);
                    response = await httpClient.PostAsync(uri, new FormUrlEncodedContent(postParameters));
                    break;
                default:
                    return null;
            }

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var newAuthorization = await GetNewSpotifyAccessToken(listener);
                listener = db.Listeners.Find(listener.ListenerId);
                listener.AccessToken = newAuthorization.access_token;
                db.SaveChanges();
                return await SendSpotifyHttpRequest(url, type, listener, postParameters);
            }
            else
            {
                return response;
            }
        }
    }
}