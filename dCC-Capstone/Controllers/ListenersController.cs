using Capstone.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        [ValidateAntiForgeryToken]
        public ActionResult Create(Listener listener)
        {
            if (ModelState.IsValid)
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
            return View();
        }
        public ActionResult SpotifyLogin()
        {
            ViewBag.SpotifyAuthorizationUri = SpotifyInteractionController.GetSpotifyAuthorization();
            return View();
        }
        public async Task<ActionResult> AuthResponse(string code, string state)
        {
            var currentListener = GetCurrentListener();
            var result = await SpotifyInteractionController.PostSpotifyOauthToReceiveSpotifyAuthAndRefreshToken(code, state, currentListener);
            
            currentListener.AccessToken = result.access_token;
            currentListener.RefreshToken = result.refresh_token;
            db.SaveChanges();

            return RedirectToAction("PickArtists");
        }
        public async Task<ActionResult> PickArtists()
        {
            var currentListener = GetCurrentListener();
            var artists = new List<Artist>();
            var genres = new List<Genre>();
            if (!db.Genres.Any())
            {
                await SpotifyInteractionController.SpotifyGenerateGenreSeeds(currentListener);
            }
            genres.AddRange(db.Genres.ToList());
            for(int i = 0; i < 15; i++)
            {
                var genre = genres[Randomness.RandomInt(0, genres.Count)];
                var artist = await SpotifyInteractionController.SpotifySearchForArtistInGenre(genre, currentListener);
                if (artist != null && 
                    artists.FirstOrDefault(a=>a.ArtistSpotifyId == artist.ArtistSpotifyId) is null)
                {
                    artists.Add(artist);
                }
            }
            return View(artists);
        }

        [HttpPost]
        public ActionResult PickArtists(List<Artist> artists)
        {
            var listenerInDb = GetCurrentListener();
            foreach (var artist in artists)
            {
                if (artist.Checked)
                {
                    artist.Checked = false;
                    Artist artistInDb = db.Artists.FirstOrDefault(a => a.ArtistSpotifyId == artist.ArtistSpotifyId);

                    if (artistInDb is null)
                    {
                        artistInDb = db.Artists.Add(artist);
                        db.SaveChanges();
                    }

                    ListenerArtist listenerArtistInDb = db.ListenerArtists.FirstOrDefault(la => la.Artist.ArtistSpotifyId == artist.ArtistSpotifyId && la.ListenerId == listenerInDb.ListenerId);

                    if (listenerArtistInDb is null)
                    {
                        db.ListenerArtists.Add(new ListenerArtist() { ListenerId = listenerInDb.ListenerId, ArtistId = artistInDb.ArtistId, ArtistLiked = true });
                    }
                    else
                    {
                        listenerArtistInDb.ArtistLiked = true;
                    }
                }
            }
            db.SaveChanges();
            
            return RedirectToAction("Index");
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
        public Listener GetCurrentListener()
        {
            string userGuid = User.Identity.GetUserId();
            return db.Listeners.FirstOrDefault(l => l.UserGuid == userGuid);
        }
    }
}
