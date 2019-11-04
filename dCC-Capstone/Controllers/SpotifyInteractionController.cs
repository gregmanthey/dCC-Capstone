
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Capstone.Models;
using Newtonsoft.Json;
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
                        
                        var artistGenreString = artistItem.genres[i];
                        var genreInDb = db.Genres.FirstOrDefault(g => g.GenreSpotifyName == artistGenreString);
                        if (genreInDb is null)
                        {
                        var artistGenre = new Genre() { GenreSpotifyName = artistGenreString };
                        genreInDb = db.Genres.Add(artistGenre);
                            db.SaveChanges();
                        }
                        artistGenres.Add(genreInDb);
                    }

                    if (artistItem.images.Length > 0)
                    {
                        var newArtist = new Artist()
                        {
                            ArtistId = 0,
                            ArtistName = artistItem.name,
                            ArtistSpotifyId = artistItem.id,
                            ArtistPopularity = artistItem.popularity,
                            ArtistGenres = artistGenres,
                            ArtistImageUrl = artistItem.images[0].url,
                            ArtistSpotifyUrl = artistItem.external_urls.spotify,
                            ArtistTopTrackPreviewUrl = songPreviewUrl,
                            SearchedGenre = genre.GenreSpotifyName
                        };
                        return newArtist;
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

        public async static Task<List<Track>> SpotifySearchForRecommendedTracks(Listener listener, Playlist playlist)
        {
            if (listener is null)
            {
                Console.WriteLine("Error: Authenticated listener is required to make API call.");
                return null;
            }
            string trackLimitNumber = "5";
            StringBuilder urlBuilder = new StringBuilder($"https://api.spotify.com/v1/recommendations");
            urlBuilder.Append("?limit=" + trackLimitNumber);
            urlBuilder.Append("&target_popularity=" + playlist.PopularityTarget);
            urlBuilder.Append("&market=US");

            var genreSeeds = listener.ListenerGenres.Where(g => g.IsSpotifyGenreSeed == true).Select(g => g.GenreSpotifyName).ToList();
            genreSeeds.AddRange(listener.ListenerArtists.Where(a => !String.IsNullOrEmpty(a.SearchedGenre)).Select(a => a.SearchedGenre).ToList());

            if (genreSeeds.Count() > 0)
            {
                urlBuilder.Append("&seed_genres=");
                //bool prependComma = false;
                //for (int i = 0; i < 5; i++)
                //{

                    int randomIndex = Randomness.RandomInt(0, genreSeeds.Count());
                    //if (prependComma)
                    //{
                    //    urlBuilder.Append(",");
                    //}
                    urlBuilder.Append(genreSeeds[randomIndex]);
                    //prependComma = true;
                //}
            }

            else if (listener.ListenerArtists.Count > 0)
            {
                urlBuilder.Append("&seed_artists=");
                bool prependComma = false;
                for (int i = 0; i < 1; i++)
                {
                    int randomIndex = Randomness.RandomInt(0, listener.ListenerArtists.Count);
                    if (prependComma)
                    {
                        urlBuilder.Append(",");
                    }
                    urlBuilder.Append(listener.ListenerArtists[randomIndex].ArtistSpotifyId);
                    prependComma = true;
                }
            }

            else if (listener.ListenerTracks.Count > 0)
            {
                urlBuilder.Append("&seed_tracks=");
                bool prependComma = false;
                for (int i = 0; i < 5; i++)
                {
                    int randomIndex = Randomness.RandomInt(0, listener.ListenerTracks.Count);
                    if (prependComma)
                    {
                        urlBuilder.Append(",");
                    }
                    urlBuilder.Append(listener.ListenerTracks[randomIndex].TrackSpotifyId);
                    prependComma = true;
                }
            }

            else
            {
                Console.WriteLine("Error, we need to have a liked Artist, Genre, or Track to generate a playlist");
                return null;
            }


            if (playlist.Mood != null)
            {
                if(playlist.Mood.MoodValenceMinimum != null)
                    urlBuilder.Append("&min_valence=" + playlist.Mood.MoodValenceMinimum.ToString());
                if (playlist.Mood.MoodValenceMaximum != null)
                    urlBuilder.Append("&max_valence=" + playlist.Mood.MoodValenceMaximum.ToString());
                if (playlist.Mood.MoodValenceTarget != null)
                    urlBuilder.Append("&target_valence=" + playlist.Mood.MoodValenceTarget.ToString());

                if (playlist.Mood.MoodTempoMinimum != null)
                    urlBuilder.Append("&min_tempo=" + playlist.Mood.MoodTempoMinimum.ToString());
                if (playlist.Mood.MoodTempoMaximum != null)
                    urlBuilder.Append("&max_tempo=" + playlist.Mood.MoodTempoMaximum.ToString());
                if (playlist.Mood.MoodTempoTarget != null)
                    urlBuilder.Append("&target_tempo=" + playlist.Mood.MoodTempoTarget.ToString());

                if (playlist.Mood.MoodEnergyMinimum != null)
                    urlBuilder.Append("&min_energy=" + playlist.Mood.MoodEnergyMinimum.ToString());
                if (playlist.Mood.MoodEnergyMaximum != null)
                    urlBuilder.Append("&max_energy=" + playlist.Mood.MoodEnergyMaximum.ToString());
                if (playlist.Mood.MoodEnergyTarget != null)
                    urlBuilder.Append("&target_energy=" + playlist.Mood.MoodEnergyTarget.ToString());

                if (playlist.Mood.MoodDanceabilityMinimum != null)
                    urlBuilder.Append("&min_danceability=" + playlist.Mood.MoodDanceabilityMinimum.ToString());
                if (playlist.Mood.MoodDanceabilityMinimum != null)
                    urlBuilder.Append("&max_danceability=" + playlist.Mood.MoodDanceabilityMaximum.ToString());
                if (playlist.Mood.MoodDanceabilityTarget != null)
                    urlBuilder.Append("&target_danceability=" + playlist.Mood.MoodDanceabilityTarget.ToString());

                if (playlist.Mood.MoodAcousticnessMinimum != null)
                    urlBuilder.Append("&min_acousticness=" + playlist.Mood.MoodAcousticnessMinimum.ToString());
                if (playlist.Mood.MoodAcousticnessMinimum != null)
                    urlBuilder.Append("&max_acousticness=" + playlist.Mood.MoodAcousticnessMaximum.ToString());
                if (playlist.Mood.MoodAcousticnessTarget != null)
                    urlBuilder.Append("&target_acousticness=" + playlist.Mood.MoodAcousticnessTarget.ToString());

                if (playlist.Mood.MoodSpeechinessMinimum != null)
                    urlBuilder.Append("&min_speechiness=" + playlist.Mood.MoodSpeechinessMinimum.ToString());
                if (playlist.Mood.MoodSpeechinessMinimum != null)
                    urlBuilder.Append("&max_speechiness=" + playlist.Mood.MoodSpeechinessMaximum.ToString());
                if (playlist.Mood.MoodSpeechinessTarget != null)
                    urlBuilder.Append("&target_speechiness=" + playlist.Mood.MoodSpeechinessTarget.ToString());

                if (playlist.Mood.MoodInstrumentalnessMinimum != null)
                    urlBuilder.Append("&min_instrumentalness=" + playlist.Mood.MoodInstrumentalnessMinimum.ToString());
                if (playlist.Mood.MoodInstrumentalnessMinimum != null)
                    urlBuilder.Append("&max_instrumentalness=" + playlist.Mood.MoodInstrumentalnessMaximum.ToString());
                if (playlist.Mood.MoodInstrumentalnessTarget != null)
                    urlBuilder.Append("&target_instrumentalness=" + playlist.Mood.MoodInstrumentalnessTarget.ToString());

                if (playlist.Mood.MoodLivenessMinimum != null)
                    urlBuilder.Append("&min_liveness=" + playlist.Mood.MoodLivenessMinimum.ToString());
                if (playlist.Mood.MoodLivenessMinimum != null)
                    urlBuilder.Append("&max_liveness=" + playlist.Mood.MoodLivenessMaximum.ToString());
                if (playlist.Mood.MoodLivenessTarget != null)
                    urlBuilder.Append("&target_liveness=" + playlist.Mood.MoodLivenessTarget.ToString());

                if (playlist.Mood.MoodLoudnessMinimum != null)
                    urlBuilder.Append("&min_loudness=" + playlist.Mood.MoodLoudnessMinimum.ToString());
                if (playlist.Mood.MoodLoudnessMinimum != null)
                    urlBuilder.Append("&max_loudness=" + playlist.Mood.MoodLoudnessMaximum.ToString());

                if (playlist.Mood.IsInMajorKeyMood != null)
                    urlBuilder.Append("&target_mode=" + playlist.Mood.IsInMajorKeyMood.ToString());
            }
            if (playlist.DynamicTracksOnly)
            {
                urlBuilder.Append("&max_loudness=-9");
            }

            string url = urlBuilder.ToString();
            var response = await SendSpotifyHttpRequest(url, "GET", listener);
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var recommendedTracks = JsonConvert.DeserializeObject<SpotifyRecommendedTracksJsonResponse.Rootobject>(jsonResponse);
            List<Track> playlistTracks = new List<Track>();
            try
            {
                foreach (var track in recommendedTracks.tracks)
                {


                    Track newTrack = new Track()
                    {
                        TrackId = 0,
                        TrackAlbumSpotifyId = track.album.id,
                        TrackSpotifyId = track.id,
                        TrackName = track.name,
                        TrackPopularity = track.popularity,
                        TrackSpotifyUrl = track.external_urls.spotify,
                        TrackPreviewUrl = track.preview_url,
                        TrackDurationInMs = track.duration_ms,
                        TrackNumber = track.track_number,
                        TrackDiscNumber = track.disc_number
                    };

                    var trackArtist = track.artists[0];
                    Artist artistInDb = db.Artists.FirstOrDefault(a => a.ArtistSpotifyId == trackArtist.id);

                    if (artistInDb is null)
                    {
                        artistInDb = new Artist()
                        {
                            ArtistId = 0,
                            ArtistName = trackArtist.name,
                            ArtistSpotifyId = trackArtist.id,
                            ArtistSpotifyUrl = trackArtist.external_urls.spotify
                        };
                        artistInDb = db.Artists.Add(artistInDb);
                        db.SaveChanges();
                    }
                    newTrack.TrackArtistId = artistInDb.ArtistId;

                    var albumArtist = track.album.artists[0];
                    var albumArtistInDb = db.Artists.FirstOrDefault(a => a.ArtistSpotifyId == albumArtist.id);

                    if (albumArtistInDb is null)
                    {
                        albumArtistInDb = new Artist()
                        {
                            ArtistId = 0,
                            ArtistName = albumArtist.name,
                            ArtistSpotifyId = albumArtist.id,
                            ArtistSpotifyUrl = albumArtist.external_urls.spotify
                        };
                        albumArtistInDb = db.Artists.Add(albumArtistInDb);
                        db.SaveChanges();
                    }

                    var album = track.album;
                    var albumInDb = db.Albums.FirstOrDefault(a => a.AlbumSpotifyId == album.id);

                    if (albumInDb is null)
                    {
                        albumInDb = new Album()
                        {
                            AlbumId = 0,
                            AlbumArtistId = albumArtistInDb.ArtistId,
                            AlbumSpotifyId = album.id,
                            AlbumImageUrl = album.images[0].url,
                            AlbumName = album.name,
                            AlbumSpotifyUrl = album.external_urls.spotify,
                            AlbumTotalTracks = album.total_tracks
                        };
                        albumInDb = db.Albums.Add(albumInDb);
                        db.SaveChanges();
                    }
                    newTrack.TrackAlbumId = albumInDb.AlbumId;

                    Track newTrackInDb = db.Tracks.FirstOrDefault(t => t.TrackSpotifyId == track.id);
                    if (newTrackInDb is null)
                    {
                        newTrackInDb = db.Tracks.Add(newTrack);
                        db.SaveChanges();
                    }

                    if (albumInDb.AlbumTracks is null)
                    {
                        albumInDb.AlbumTracks = new List<Track>();
                    }

                    var albumTrackInDb = albumInDb.AlbumTracks.FirstOrDefault(t => t.TrackSpotifyId == newTrackInDb.TrackSpotifyId);
                    if (albumTrackInDb is null)
                    {
                        albumInDb.AlbumTracks.Add(newTrackInDb);
                        db.SaveChanges();
                    }

                    playlistTracks.Add(newTrackInDb);
                }

                return playlistTracks;
            }
            catch { return null; }
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
                    Genre newGenre = new Genre { GenreId = 0, GenreSpotifyName = genre, IsSpotifyGenreSeed = true };
                    db.Genres.Add(newGenre);
                }
                db.SaveChanges();
            }
            var genres = new List<Genre>(db.Genres.ToList());
            return genres;
        }

        public async static Task<Track> GetSpotifyTrackDetails(Listener listener, Track track)
        {
            string url = $"https://api.spotify.com/v1/audio-features/{track.TrackSpotifyId}";
            var content = await SendSpotifyHttpRequest(url, "GET", listener);
            var jsonResponse = await content.Content.ReadAsStringAsync();
            var audioFeatures = JsonConvert.DeserializeObject<SpotifyTrackAudioFeaturesJsonResponse.Audio_Features>(jsonResponse);
            track.TrackDanceability = audioFeatures.danceability;
            track.TrackEnergy = audioFeatures.energy;
            track.TrackLoudness = audioFeatures.loudness;
            track.TrackIsInMajorKey = audioFeatures.mode;
            track.TrackValence = audioFeatures.valence;
            track.TrackTempo = audioFeatures.tempo;
            track.TrackAcousticness = audioFeatures.acousticness;
            track.TrackInstrumentalness = audioFeatures.instrumentalness;
            return track;
        }

        public async static Task<Album> GetSpotifyAlbumDetails(Listener listener, Album album)
        {
            string url = $"https://api.spotify.com/v1/albums/{album.AlbumSpotifyId}";
            var content = await SendSpotifyHttpRequest(url, "GET", listener);
            var jsonResponse = await content.Content.ReadAsStringAsync();
            var spotifyAlbum = JsonConvert.DeserializeObject<SpotifyAlbumDetailsJsonResponse.Rootobject>(jsonResponse);
            album.AlbumTotalTracks = spotifyAlbum.total_tracks;
            if (spotifyAlbum.images.Length > 0)
            {
                album.AlbumImageUrl = spotifyAlbum.images[0].url;
            }

            if (album.AlbumTracks.Count != album.AlbumTotalTracks)
            {
                for (int i = 0; i < spotifyAlbum.tracks.items.Length; i++)
                {
                    var track = spotifyAlbum.tracks.items[i];
                        Track newTrack = new Track()
                        {
                            TrackId = 0,
                            TrackAlbumSpotifyId = spotifyAlbum.id,
                            TrackSpotifyId = track.id,
                            TrackName = track.name,
                            TrackSpotifyUrl = track.external_urls.spotify,
                            TrackPreviewUrl = track.preview_url,
                            TrackDurationInMs = track.duration_ms,
                            TrackNumber = track.track_number,
                            TrackDiscNumber = track.disc_number
                        };
                    var trackInDb = db.Tracks.Include("Album").Include("Artist").FirstOrDefault(t => t.TrackSpotifyId == track.id);
                    if (trackInDb != null)
                    {
                        newTrack = trackInDb;
                    }
                    Artist newArtist = new Artist();
                    if (track.artists != null)
                    {
                        var trackArtist = track.artists[0];
                        newArtist = new Artist()
                    {
                        ArtistId = 0,
                        ArtistName = trackArtist.name,
                        ArtistSpotifyId = trackArtist.id,
                        ArtistSpotifyUrl = trackArtist.external_urls.spotify
                    };
                        var trackArtistInDb = db.Artists.FirstOrDefault(a => a.ArtistSpotifyId == trackArtist.id);
                        if (trackArtistInDb != null)
                        {
                            newArtist = trackArtistInDb;
                        }
                        else
                        {
                            newArtist = db.Artists.Add(newArtist);
                        }
                    }

                    if (album.AlbumTracks.FirstOrDefault(t => t.TrackSpotifyId == newTrack.TrackSpotifyId) is null)
                    {
                        newTrack.Album = album;
                        newTrack.Artist = newArtist;
                        album.AlbumTracks.Add(newTrack);
                    }

                }
            }

            //db.SaveChanges();
            return album;
        }

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