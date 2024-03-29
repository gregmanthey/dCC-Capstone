﻿using System;
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
    public class AlbumsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Albums
        public async Task<ActionResult> Index()
        {
            var albums = db.Albums.Include(a => a.Artist);
            return View(await albums.ToListAsync());
        }

        // GET: Albums/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Album albumInDb = await db.Albums.Include(a=>a.AlbumTracks).Include(a=>a.Artist).FirstOrDefaultAsync(a=>a.AlbumId == id);
            if (albumInDb == null)
            {
                return HttpNotFound();
            }

            if (albumInDb.AlbumTracks.Count != albumInDb.AlbumTotalTracks)
            {
                string userGuid = User.Identity.GetUserId();
                var currentListener = db.Listeners.FirstOrDefault(l => l.UserGuid == userGuid);
                if (currentListener is null)
                {
                    Console.WriteLine("User is not logged in");
                    return RedirectToAction("Index", "Home");
                }
                albumInDb = await SpotifyInteractionController.GetSpotifyAlbumDetails(currentListener, albumInDb);
                
            }

            return View(albumInDb);
        }

        // GET: Albums/Create
        public ActionResult Create()
        {
            ViewBag.AlbumArtistId = new SelectList(db.Artists, "ArtistId", "ArtistSpotifyId");
            return View();
        }

        // POST: Albums/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "AlbumId,AlbumName,AlbumSpotifyId,AlbumSpotifyUrl,AlbumImageUrl,AlbumArtistId")] Album album)
        {
            if (ModelState.IsValid)
            {
                db.Albums.Add(album);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.AlbumArtistId = new SelectList(db.Artists, "ArtistId", "ArtistSpotifyId", album.AlbumArtistId);
            return View(album);
        }

        // GET: Albums/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Album album = await db.Albums.FindAsync(id);
            if (album == null)
            {
                return HttpNotFound();
            }
            ViewBag.AlbumArtistId = new SelectList(db.Artists, "ArtistId", "ArtistSpotifyId", album.AlbumArtistId);
            return View(album);
        }

        // POST: Albums/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "AlbumId,AlbumName,AlbumSpotifyId,AlbumSpotifyUrl,AlbumImageUrl,AlbumArtistId")] Album album)
        {
            if (ModelState.IsValid)
            {
                db.Entry(album).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.AlbumArtistId = new SelectList(db.Artists, "ArtistId", "ArtistSpotifyId", album.AlbumArtistId);
            return View(album);
        }

        // GET: Albums/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Album album = await db.Albums.FindAsync(id);
            if (album == null)
            {
                return HttpNotFound();
            }
            return View(album);
        }

        // POST: Albums/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Album album = await db.Albums.FindAsync(id);
            db.Albums.Remove(album);
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
