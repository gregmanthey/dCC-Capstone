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
            genres.AddRange(db.Genres.AsNoTracking().ToList());
            List<Task<Artist>> artistTask = new List<Task<Artist>>();
            for(int i = 0; i < 25; i++)
            {
                var genre = genres[Randomness.RandomInt(0, genres.Count)];
                artistTask.Add(SpotifyInteractionController.SpotifySearchForArtistInGenre(genre, currentListener));
            }

            foreach (var task in artistTask)
            {
                var artist = await task;
                if (artist != null &&
                    artists.FirstOrDefault(a => a.ArtistSpotifyId == artist.ArtistSpotifyId) is null)
                {
                    artists.Add(artist);
                }
            }
            return View(artists);
        }

        [HttpPost]
        public ActionResult PickArtists(List<Artist> artists)
        {
            string userGuid = User.Identity.GetUserId();
            var listenerInDb =  db.Listeners.Include("ListenerArtists").Include("ListenerGenres").FirstOrDefault(l => l.UserGuid == userGuid);
            foreach (var artist in artists)
            {
                if (artist.ArtistChecked)
                {
                    artist.ArtistChecked = false;
                    for (int i = artist.ArtistGenres.Count - 1; i > 0; i--)
                    {
                        Genre genre = artist.ArtistGenres[i];
                        var genreInDb = db.Genres.FirstOrDefault(g => g.GenreSpotifyName == genre.GenreSpotifyName);
                        if (genreInDb is null)
                        {
                            genreInDb = db.Genres.Add(genre);
                            db.SaveChanges();
                            listenerInDb.ListenerGenres.Add(genreInDb);
                        }
                        else if (listenerInDb.ListenerGenres.FirstOrDefault(g => g.GenreId == genreInDb.GenreId) is null)
                        {
                            listenerInDb.ListenerGenres.Add(genreInDb);
                        }
                        genre = genreInDb;
                    }
                    Artist artistInDb = db.Artists.FirstOrDefault(a => a.ArtistSpotifyId == artist.ArtistSpotifyId);

                    if (artistInDb is null)
                    {
                        artistInDb = db.Artists.Add(artist);
                        db.SaveChanges();
                    }
                    if (listenerInDb.ListenerArtists.FirstOrDefault(la => la.ArtistSpotifyId == artistInDb.ArtistSpotifyId) is null)
                    {
                        listenerInDb.ListenerArtists.Add(artistInDb);
                        artistInDb.ArtistListeners.Add(listenerInDb);
                    }
                }
            }
            db.SaveChanges();
            
            return RedirectToAction("Index", "Home");
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
        private Listener GetCurrentListener()
        {
            string userGuid = User.Identity.GetUserId();
            return db.Listeners.FirstOrDefault(l => l.UserGuid == userGuid);
        }
    }
}
