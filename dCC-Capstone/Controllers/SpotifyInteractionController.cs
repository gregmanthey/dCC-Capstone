
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
            try
            {
                var artistItem = artistsRootobject.artists.items[0];
                List<Genre> artistGenres = new List<Genre>();
                for (int i = 0; i < artistItem.genres.Length; i++)
                {
                    artistGenres.Add(new Genre() { GenreSpotifyName = artistItem.genres[i] });
                }
                
                Artist artist = new Artist() {
                    ArtistName = artistItem.name,
                    ArtistSpotifyId = artistItem.uri,
                    Popularity = artistItem.popularity,
                    ArtistGenres = artistGenres,
                    ArtistImageUrl = artistItem.images[0].url,
                    ArtistSpotifyUrl = artistItem.external_urls.spotify };

                return artist;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async static Task<IList<Genre>> SpotifyGenerateGenreSeeds(Listener listener)
        {
            //GET https://api.spotify.com/v1/search
            //Authorization: Bearer {access token}

            string url = "https://api.spotify.com/v1/recommendations/available-genre-seeds";
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
