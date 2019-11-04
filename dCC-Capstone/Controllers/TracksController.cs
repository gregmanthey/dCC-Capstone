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

namespace Capstone.Controllers
{
    public class TracksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Tracks
        public async Task<ActionResult> Index()
        {
            //var userGuid = User.Identity.GetUserId();
            //var currentUser = db.Listeners.FirstOrDefault(l => l.UserGuid == userGuid);
            //var userTracks = db.Playlists.Include(p => p.PlaylistTracks).Where(p => p.CreatedBy == currentUser.ListenerId).SelectMany(t => t.PlaylistTracks);
            
            return View(await db.Tracks.ToListAsync());
        }

        // GET: Tracks/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var userGuid = User.Identity.GetUserId();
            var currentListener = db.Listeners.FirstOrDefault(l => l.UserGuid == userGuid);
            Track trackInDb = await db.Tracks.Include(t => t.Album).Include(t => t.Artist).FirstOrDefaultAsync(t => t.TrackId == id);
            
            if (trackInDb == null)
            {
                return HttpNotFound();
            }

            if (trackInDb.TrackAcousticness is null)
            {
                trackInDb = await SpotifyInteractionController.GetSpotifyTrackDetails(currentListener, trackInDb);
                db.SaveChanges();
            }

            return View(trackInDb);
        }

        // GET: Tracks/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tracks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Track track)
        {
                var trackInDb = db.Tracks.Add(track);
                await db.SaveChangesAsync();
                return RedirectToAction("Details", new { id = trackInDb.TrackId});
        }

        // GET: Tracks/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Track track = await db.Tracks.FindAsync(id);
            if (track == null)
            {
                return HttpNotFound();
            }
            return View(track);
        }

        // POST: Tracks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "TrackId,TrackSpotifyId,TrackName,TrackValence,TrackEnergy,TrackDanceability,TrackLoudness,TrackTempo,TrackDurationInMs,TrackIsInMajorKey,TrackLiked,TrackDisliked")] Track track)
        {
            if (ModelState.IsValid)
            {
                db.Entry(track).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(track);
        }

        // GET: Tracks/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Track track = await db.Tracks.FindAsync(id);
            if (track == null)
            {
                return HttpNotFound();
            }
            return View(track);
        }

        // POST: Tracks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Track track = await db.Tracks.FindAsync(id);
            db.Tracks.Remove(track);
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
