
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
using Capstone.Models;
using Newtonsoft.Json;

namespace Capstone.Controllers
{
    public class SingleHttpClientInstanceController : ApiController
    {
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

        public static SpotifyAuthorizationTokenResponse.Rootobject PostSpotifyOauthToReceiveSpotifyAuthAndRefreshToken(string code, string state)
        {
            string url = "https://accounts.spotify.com/api/token";
            Uri uri = new Uri(url);
            var parameters = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("client_id", Keys.SpotifyClientId),
                new KeyValuePair<string, string>("client_secret", Keys.SpotifyClientSecret),
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                new KeyValuePair<string, string>("code", code),
                new KeyValuePair<string, string>("redirect_uri", redirect_url)
            };
            var content = GetContentAsync(url, "POST", parameters);
            var token = JsonConvert.DeserializeObject<SpotifyAuthorizationTokenResponse.Rootobject>(content);
            return token;
        }

        public static SpotifyAuthorizationTokenResponse.Rootobject GetNewSpotifyAccessToken(string refresh_token)
        {
            string url = "https://accounts.spotify.com/api/token";
            Uri uri = new Uri(url);
            var parameters = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", "refresh_token"),
                new KeyValuePair<string, string>("refresh_token", refresh_token)
            };
            var content = GetContentAsync(url, "POST", parameters);
            var token = JsonConvert.DeserializeObject<SpotifyAuthorizationTokenResponse.Rootobject>(content);
            return token;
        }
        private static string GetContentAsync(string url,
        string method = "POST",
        IEnumerable<KeyValuePair<string, string>> parameters = null)
        {
            return method == "POST" ? PostAsync(url, parameters) : GetAsync(url, parameters);
        }

        private static string PostAsync(string url, IEnumerable<KeyValuePair<string, string>> parameters = null)
        {
            var uri = new Uri(url);

            var request = WebRequest.Create(uri) as HttpWebRequest;
            request.Method = "POST";
            request.KeepAlive = true;
            request.ContentType = "application/x-www-form-urlencoded";

            var postParameters = GetPostParameters(parameters);

            var bs = Encoding.UTF8.GetBytes(postParameters);
            using (var reqStream = request.GetRequestStream())
            {
                reqStream.Write(bs, 0, bs.Length);
            }

            using (var response = request.GetResponse())
            {
                var sr = new StreamReader(response.GetResponseStream());
                var jsonResponse = sr.ReadToEnd();
                sr.Close();

                return jsonResponse;
            }
        }

        private static string GetPostParameters(IEnumerable<KeyValuePair<string, string>> parameters = null)
        {
            var postParameters = string.Empty;
            foreach (var parameter in parameters)
            {
                postParameters += string.Format("&{0}={1}", parameter.Key,
                    HttpUtility.HtmlEncode(parameter.Value));
            }
            postParameters = postParameters.Substring(1);

            return postParameters;
        }

        private static string GetAsync(string url, IEnumerable<KeyValuePair<string, string>> parameters = null)
        {
            url += "?" + GetQueryStringParameters(parameters);

            var forIdsWebRequest = WebRequest.Create(url);
            using (var response = (HttpWebResponse)forIdsWebRequest.GetResponse())
            {
                using (var data = response.GetResponseStream())
                using (var reader = new StreamReader(data))
                {
                    var jsonResponse = reader.ReadToEnd();

                    return jsonResponse;
                }
            }
        }

        private static string GetQueryStringParameters(IEnumerable<KeyValuePair<string, string>> parameters = null)
        {
            var queryStringParameters = string.Empty;
            foreach (var parameter in parameters)
            {
                queryStringParameters += string.Format("&{0}={1}", parameter.Key,
                    HttpUtility.HtmlEncode(parameter.Value));
            }
            queryStringParameters = queryStringParameters.Substring(1);

            return queryStringParameters;
        }
    }
}
