
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
    public class SingleHttpClientInstanceController : Controller
    {
        private static ApplicationDbContext db = new ApplicationDbContext();
        private static HttpClient httpClient = new HttpClient();
        private static string state;
        private const string redirect_url = "https://localhost:44353/Listeners/AuthResponse";

        public static Uri GetSpotifyAuthorization()
        {
            state = "abcd0987qwer1234";//random string length 16 (arbitrary)
            var url = $"https://accounts.spotify.com/authorize?client_id={Keys.SpotifyClientId}&response_type=code&redirect_uri={redirect_url}&scope=playlist-modify-private,user-read-private&state={state}";
            Uri uri = new Uri(url);
            return uri;
        }

        public async static Task<SpotifyAuthorizationTokenResponse.Rootobject> PostSpotifyOauthToReceiveSpotifyAuthAndRefreshToken(string code, string state)
        {
            string url = "https://accounts.spotify.com/api/token";
            var combinedId = Convert.ToBase64String(Encoding.UTF8.GetBytes(Keys.SpotifyClientId + ":" + Keys.SpotifyClientSecret));
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", combinedId);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var parameters = new Dictionary<string, string>();
            //parameters.Add("client_id", Keys.SpotifyClientId);
            //parameters.Add("client_secret", Keys.SpotifyClientSecret);
            parameters.Add("grant_type", "authorization_code");
            parameters.Add("code", code);
            parameters.Add("redirect_uri", redirect_url);
            //string json = JsonConvert.SerializeObject(parameters);
            //var httpContent = new StringContent(json, Encoding.Unicode, "application/x-www-form-urlencoded");
            Uri uri = new Uri(url);
            var content = await httpClient.PostAsync(uri, new FormUrlEncodedContent(parameters));//GetContentAsync(url, "POST", parameters);
            var jsonResponse = await content.Content.ReadAsStringAsync();
            var token = JsonConvert.DeserializeObject<SpotifyAuthorizationTokenResponse.Rootobject>(jsonResponse);
            return token;
        }

        public async static Task<SpotifyAuthorizationTokenResponse.Rootobject> GetNewSpotifyAccessToken(string refresh_token)
        {
            string url = "https://accounts.spotify.com/api/token";
            var combinedId = Convert.ToBase64String(Encoding.UTF8.GetBytes(Keys.SpotifyClientId + ":" + Keys.SpotifyClientSecret));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", combinedId);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var parameters = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", "refresh_token"),
                new KeyValuePair<string, string>("refresh_token", refresh_token)
            };
            //string json = JsonConvert.SerializeObject(parameters);
            //var httpContent = new StringContent(json, Encoding.UTF8, "application/x-www-form-urlencoded");
            Uri uri = new Uri(url);
            var content = await httpClient.PostAsync(uri, new FormUrlEncodedContent(parameters));//GetContentAsync(url, "POST", parameters);
            var jsonResponse = await content.Content.ReadAsStringAsync();
            //var content = GetContentAsync(url, "POST", parameters);
            var token = JsonConvert.DeserializeObject<SpotifyAuthorizationTokenResponse.Rootobject>(jsonResponse);
            return token;
        }

        public async static Task<Artist> SpotifySearchForTopArtistInGenre(Genre genre, string accessToken, string refreshToken)
        {
            if (genre is null)
            {
                return null;
            }
            //GET https://api.spotify.com/v1/search
            //Authorization: Bearer {access token}
            string url = $"https://api.spotify.com/v1/search?q=genre:{genre.GenreSpotifyName}&type=artist&limit=1";
            //Uri genreUri = new Uri(genre.GenreSpotifyName);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            //var parameters = new Dictionary<string, string>();

            //parameters.Add("q", "genre:" + genre);
            //parameters.Add("limit", "1");
            //parameters.Add("type", "artist");


            //string json = JsonConvert.SerializeObject(parameters);
            //var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            Uri uri = new Uri(url);
            var content = await httpClient.GetAsync(uri);//, new FormUrlEncodedContent(parameters));//GetContentAsync(url, "POST", parameters);
            if (content.StatusCode == HttpStatusCode.Unauthorized)
            {
                await GetNewSpotifyAccessToken(refreshToken);
                return await SpotifySearchForTopArtistInGenre(genre, accessToken, refreshToken);
            }
            var jsonResponse = await content.Content.ReadAsStringAsync();
            //var content = GetContentAsync(url, "GET", parameters);
            var artistsRootobject = JsonConvert.DeserializeObject<SpotifyArtistsSearchJsonResponse.Rootobject>(jsonResponse);
            try
            {
                var artistItem = artistsRootobject.artists.items[0];
                Artist artist = new Artist() { ArtistName = artistItem.name, ArtistSpotifyId = artistItem.uri, Popularity = artistItem.popularity };

                return artist;
            }
            catch (Exception)
            {

                return null;
            }
        }

        public async static Task SpotifyGenerateGenres(string accessToken, string refreshToken)
        {
            //GET https://api.spotify.com/v1/search
            //Authorization: Bearer {access token}
            //genre.GenreSpotifyName;
            string url = "https://api.spotify.com/v1/recommendations/available-genre-seeds";
            httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
            //var content = GetContentAsync(url, "GET", parameters);
            //string json = JsonConvert.SerializeObject(parameters);
            //var httpContent = new StringContent(json, Encoding.UTF8, "application/x-www-form-urlencoded");
            Uri uri = new Uri(url);
            var content = await httpClient.GetAsync(uri);//GetContentAsync(url, "POST", parameters);
            if (content.StatusCode == HttpStatusCode.Unauthorized)
            {
                await GetNewSpotifyAccessToken(refreshToken);
                await SpotifyGenerateGenres(accessToken, refreshToken);
                return;
            }
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
            //var genres = new List<Genre>(db.Genres.ToList());
            //return genres;
        }
    }
}
