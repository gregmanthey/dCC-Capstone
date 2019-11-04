using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Capstone.Models;
using Microsoft.AspNet.Identity;
using Capstone.Models.ViewModels;

namespace Capstone.Controllers
{
    public class PlaylistsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Playlists
        public async Task<ActionResult> Index()
        {
            var playlists = db.Playlists.Include(p => p.Listener);
            return View(await playlists.ToListAsync());
        }

        // GET: Playlists/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Playlist playlist = await db.Playlists.
                Include(p => p.PlaylistTracks).
                Include(p => p.PlaylistMood).
                FirstOrDefaultAsync(p => p.PlaylistId == id);

            if (playlist == null)
            {
                return HttpNotFound();
            }
            var viewModel = new PlaylistArtistViewModel()
            {
                Playlist = playlist,
                Artists = db.Artists.ToList(),
                Albums = db.Albums.ToList()
            };
            return View(viewModel);
        }

        // GET: Playlists/Create
        public ActionResult Create()
        {
            ViewBag.Moods = new SelectList(db.Moods, "MoodId", "MoodName");

            Playlist playlist = new Playlist() 
            {
                GenreWeightPercentage = 100,
                PopularityTarget = 100
            };
            return View(playlist);
        }

        // POST: Playlists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Playlist playlist)
        {
            if (ModelState.IsValid)
            {
                if (playlist.PlaylistMood != null)
                {
                    playlist.Mood = await db.Moods.FindAsync(playlist.PlaylistMood);
                }
                var currentListener = GetCurrentListener();
                var playlistTrackTasks = new List<Task<List<Track>>>();
                var playlistTracks = new List<Track>();
                for (int i = 0; i < 30; i++)
                {
                    playlistTrackTasks.Add(SpotifyInteractionController.SpotifySearchForRecommendedTracks(currentListener, playlist));
                }
                foreach (var task in playlistTrackTasks)
                {
                    var tracks = await task;
                    if (tracks != null)
                    {
                        playlistTracks.AddRange(tracks);
                    }
                }
                
                playlist.PlaylistTracks = playlistTracks.Distinct().ToList();
                playlist.CreatedBy = currentListener.ListenerId;
                var playlistInDb = db.Playlists.Add(playlist);
                db.SaveChanges();
                return RedirectToAction("Details", new { id = playlistInDb.PlaylistId});
            }
            return RedirectToAction("Index");
        }

        // GET: Playlists/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Playlist playlist = await db.Playlists.FindAsync(id);
            if (playlist == null)
            {
                return HttpNotFound();
            }
            ViewBag.CreatedBy = new SelectList(db.Listeners, "ListenerId", "ScreenName", playlist.CreatedBy);
            return View(playlist);
        }

        // POST: Playlists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "PlaylistId,PlaylistName,CreatedBy")] Playlist playlist)
        {
            if (ModelState.IsValid)
            {
                db.Entry(playlist).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.CreatedBy = new SelectList(db.Listeners, "ListenerId", "ScreenName", playlist.CreatedBy);
            return View(playlist);
        }

        // GET: Playlists/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Playlist playlist = await db.Playlists.FindAsync(id);
            if (playlist == null)
            {
                return HttpNotFound();
            }
            return View(playlist);
        }

        // POST: Playlists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Playlist playlist = await db.Playlists.FindAsync(id);
            db.Playlists.Remove(playlist);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        
        private Listener GetCurrentListener()
        {
            var userGuid = User.Identity.GetUserId();
            return db.Listeners.Include("ListenerArtists").Include("ListenerGenres").Include("ListenerTracks").FirstOrDefault(l => l.UserGuid == userGuid);
        }
    }
}
