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
            Playlist playlist = await db.Playlists.FindAsync(id);
            if (playlist == null)
            {
                return HttpNotFound();
            }
            return View(playlist);
        }

        // GET: Playlists/Create
        public ActionResult Create()
        {
            ViewBag.Moods = new SelectList(db.Moods, "MoodId", "MoodName");
            Playlist playlist = new Playlist() {
                GenreWeightPercentage = 100,
                PopularityWeightPercentage = 100 };
            return View(playlist);
        }

        // POST: Playlists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "PlaylistId,PlaylistName,CreatedBy")] Playlist playlist)
        {
            if (ModelState.IsValid)
            {
                db.Playlists.Add(playlist);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.CreatedBy = new SelectList(db.Listeners, "ListenerId", "ScreenName", playlist.CreatedBy);
            return View(playlist);
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
    }
}
