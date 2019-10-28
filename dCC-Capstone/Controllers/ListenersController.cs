using Capstone.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Capstone.Controllers
{
    public class ListenersController : Controller
    {
        ApplicationDbContext db;
        public ListenersController()
        {
            db = new ApplicationDbContext();
        }
        // GET: Listeners
        public ActionResult Index()
        {
            return View();
        }

        // GET: Listeners/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Listeners/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Listeners/Create
        [HttpPost]
        public ActionResult Create(Listener listener)
        {
            try
            {
                // TODO: Add insert logic here
                var userGuid = User.Identity.GetUserId();
                listener.UserGuid = userGuid;
                db.Listeners.Add(listener);
                db.SaveChanges();
                return RedirectToAction("SpotifyLogin");
            }
            catch
            {
                return View();
            }
        }
        public ActionResult SpotifyLogin()
        {
            ViewBag.SpotifyAuthorizationUri = SingleHttpClientInstanceController.GetSpotifyAuthorization();
            return View();
        }
        public ActionResult AuthResponse(string code, string state)
        {
            var result = SingleHttpClientInstanceController.PostSpotifyOauthToReceiveSpotifyAuthAndRefreshToken(code, state);
            var userGuid = User.Identity.GetUserId();
            var currentListener = db.Listeners.FirstOrDefault(l => l.UserGuid == userGuid);
            currentListener.AccessToken = result.access_token;
            currentListener.RefreshToken = result.refresh_token;
            db.SaveChanges();

            return RedirectToAction("PickArtists");
        }

        // GET: Listeners/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Listeners/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Listeners/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Listeners/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
