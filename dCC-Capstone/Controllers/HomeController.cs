using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Http;
using System.Windows;

using Newtonsoft.Json.Linq;
using Owin.Security.Providers.Spotify;
using System.Net.Http.Headers;

namespace Capstone.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {            
            return View();
        }

        

        //public ActionResult SpotifyAuthorizeResponse(string code, string state)
        //{
        //    SingleHttpClientInstanceController.PostSpotifyOauthToReceiveSpotifyAuthAndRefreshToken(code, state);
        //    //if state != expected state, not valid response
        //    //if (!String.IsNullOrEmpty(error))
        //    //{
        //    //    //error, user authorization failed
        //    //    return HttpNotFound();
        //    //}

        //    //POST https://accounts.spotify.com/api/token
        //    //body must contain following properties encoded in 'application/x-www-form-urlencoded':
        //    //grant_type=authorization_code
        //    //code=code
        //    //var httpClient = new HttpClient();
        //    //String encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(SpotifyClientId + ":" + SpotifyClientSecret));
        //    //httpRequest.Headers.Add("Authorization", "Basic " + encoded);
        //    //curl -H "Authorization: Basic ZjM...zE=" -d grant_type=authorization_code -d code=MQCbtKe...44KN -d redirect_uri=https%3A%2F%2Fwww.foo.com%2Fauth https://accounts.spotify.com/api/token

        //    return View();
        //}

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}