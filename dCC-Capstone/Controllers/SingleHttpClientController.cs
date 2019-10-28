
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

        //static SingleHttpClientInstanceController()
        //{
        //    httpClient = new HttpClient();
        //}

        // This method uses the shared instance of HttpClient for every call to GetProductAsync.
        public static Uri GetSpotifyAuthorization()
        {
            state = "abcd0987qwer1234";//random string length 16 (arbitrary)
            var url = $"https://accounts.spotify.com/authorize?client_id={Keys.SpotifyClientId}&response_type=code&redirect_uri={redirect_url}&scope=playlist-modify-private,user-read-private&state={state}";
            Uri uri = new Uri(url);
            //var response = await httpClient.GetAsync(uri).ConfigureAwait(false);
            //var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            return uri;
            //One copy/paste
            //using (var client = new HttpClient())
            //{
            //    var postData = new List<KeyValuePair<string, string>>();
            //    postData.Add(new KeyValuePair<string, string>("username", _user));
            //    postData.Add(new KeyValuePair<string, string>("password", _pwd));
            //    postData.Add(new KeyValuePair<string, string>("grant_type", "password"));
            //    postData.Add(new KeyValuePair<string, string>("client_id", _clientId));
            //    postData.Add(new KeyValuePair<string, string>("client_secret", _clientSecret));

            //    HttpContent content = new FormUrlEncodedContent(postData);
            //    content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            //    var responseResult = client.PostAsync(_tokenUrl, content).Result;

            //    return responseResult.Content.ReadAsStringAsync().Result;
            //}

            //Another Copy/paste
            //HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
            //httpWebRequest.ContentType = "application/json";
            //httpWebRequest.Accept = "*/*";
            //httpWebRequest.Method = "GET";
            //httpWebRequest.Headers.Add("Authorization", "Basic reallylongstring");

            //var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            //using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            //{
            //    gta_allCustomersResponse answer = JsonConvert.DeserializeObject<gta_allCustomersResponse>(streamReader.ReadToEnd());
            //    return answer;
            //}

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
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
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

    //public static class TokenValidator
    //{
    //    /// <summary>
    //    /// Obtém um novo access token na API do google.
    //    /// </summary>
    //    /// <param name="clientId"></param>
    //    /// <param name="clientSecret"></param>
    //    /// <param name="refreshToken"></param>
    //    /// <returns></returns>
    //    public static GoogleRefreshTokenModel ValidateGoogleToken(string clientId, string clientSecret, string refreshToken)
    //    {
    //        const string url = "https://accounts.google.com/o/oauth2/token";

    //        var parameters = new List<KeyValuePair<string, string>>
    //    {
    //        new KeyValuePair<string, string>("client_id", clientId),
    //        new KeyValuePair<string, string>("client_secret", clientSecret),
    //        new KeyValuePair<string, string>("grant_type", "refresh_token"),
    //        new KeyValuePair<string, string>("refresh_token", refreshToken)
    //    };

    //        var content = GetContentAsync(url, "POST", parameters);

    //        var token = JsonConvert.DeserializeObject<GoogleRefreshTokenModel>(content);

    //        return token;
    //    }

    //    private static string GetContentAsync(string url,
    //        string method = "POST",
    //        IEnumerable<KeyValuePair<string, string>> parameters = null)
    //    {
    //        return method == "POST" ? PostAsync(url, parameters) : GetAsync(url, parameters);
    //    }

    //    private static string PostAsync(string url, IEnumerable<KeyValuePair<string, string>> parameters = null)
    //    {
    //        var uri = new Uri(url);

    //        var request = WebRequest.Create(uri) as HttpWebRequest;
    //        request.Method = "POST";
    //        request.KeepAlive = true;
    //        request.ContentType = "application/x-www-form-urlencoded";

    //        var postParameters = GetPostParameters(parameters);

    //        var bs = Encoding.UTF8.GetBytes(postParameters);
    //        using (var reqStream = request.GetRequestStream())
    //        {
    //            reqStream.Write(bs, 0, bs.Length);
    //        }

    //        using (var response = request.GetResponse())
    //        {
    //            var sr = new StreamReader(response.GetResponseStream());
    //            var jsonResponse = sr.ReadToEnd();
    //            sr.Close();

    //            return jsonResponse;
    //        }
    //    }

    //    private static string GetPostParameters(IEnumerable<KeyValuePair<string, string>> parameters = null)
    //    {
    //        var postParameters = string.Empty;
    //        foreach (var parameter in parameters)
    //        {
    //            postParameters += string.Format("&{0}={1}", parameter.Key,
    //                HttpUtility.HtmlEncode(parameter.Value));
    //        }
    //        postParameters = postParameters.Substring(1);

    //        return postParameters;
    //    }

    //    private static string GetAsync(string url, IEnumerable<KeyValuePair<string, string>> parameters = null)
    //    {
    //        url += "?" + GetQueryStringParameters(parameters);

    //        var forIdsWebRequest = WebRequest.Create(url);
    //        using (var response = (HttpWebResponse)forIdsWebRequest.GetResponse())
    //        {
    //            using (var data = response.GetResponseStream())
    //            using (var reader = new StreamReader(data))
    //            {
    //                var jsonResponse = reader.ReadToEnd();

    //                return jsonResponse;
    //            }
    //        }
    //    }

    //    private static string GetQueryStringParameters(IEnumerable<KeyValuePair<string, string>> parameters = null)
    //    {
    //        var queryStringParameters = string.Empty;
    //        foreach (var parameter in parameters)
    //        {
    //            queryStringParameters += string.Format("&{0}={1}", parameter.Key,
    //                HttpUtility.HtmlEncode(parameter.Value));
    //        }
    //        queryStringParameters = queryStringParameters.Substring(1);

    //        return queryStringParameters;
    //    }
    //}
}
