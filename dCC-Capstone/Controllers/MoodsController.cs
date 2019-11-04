using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Capstone.Models;

namespace Capstone.Controllers
{
    public class MoodsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Moods
        public ActionResult Index()
        {
            return View(db.Moods.ToList());
        }

        // GET: Moods/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mood mood = db.Moods.Find(id);
            if (mood == null)
            {
                return HttpNotFound();
            }
            return View(mood);
        }

        // GET: Moods/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Moods/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MoodId,MoodName,MoodEnergyMinimum,MoodEnergyMaximum,MoodEnergyTarget,MoodAcousticnessMinimum,MoodAcousticnessMaximum,MoodAcousticnessTarget,MoodSpeechinessMinimum,MoodSpeechinessMaximum,MoodSpeechinessTarget,MoodInstrumentalnessMinimum,MoodInstrumentalnessMaximum,MoodInstrumentalnessTarget,MoodLivenessMinimum,MoodLivenessMaximum,MoodLivenessTarget,MoodDanceabilityMinimum,MoodDanceabilityMaximum,MoodDanceabilityTarget,MoodLoudnessMinimum,MoodLoudnessMaximum,MoodLoudnessTarget,MoodTempoMinimum,MoodTempoMaximum,MoodTempoTarget,MoodValenceMinimum,MoodValenceMaximum,MoodValenceTarget,IsInMajorKeyMood")] Mood mood)
        {
            if (ModelState.IsValid)
            {
                db.Moods.Add(mood);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(mood);
        }

        // GET: Moods/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mood mood = db.Moods.Find(id);
            if (mood == null)
            {
                return HttpNotFound();
            }
            return View(mood);
        }

        // POST: Moods/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MoodId,MoodName,MoodEnergyMinimum,MoodEnergyMaximum,MoodEnergyTarget,MoodAcousticnessMinimum,MoodAcousticnessMaximum,MoodAcousticnessTarget,MoodSpeechinessMinimum,MoodSpeechinessMaximum,MoodSpeechinessTarget,MoodInstrumentalnessMinimum,MoodInstrumentalnessMaximum,MoodInstrumentalnessTarget,MoodLivenessMinimum,MoodLivenessMaximum,MoodLivenessTarget,MoodDanceabilityMinimum,MoodDanceabilityMaximum,MoodDanceabilityTarget,MoodLoudnessMinimum,MoodLoudnessMaximum,MoodLoudnessTarget,MoodTempoMinimum,MoodTempoMaximum,MoodTempoTarget,MoodValenceMinimum,MoodValenceMaximum,MoodValenceTarget,IsInMajorKeyMood")] Mood mood)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mood).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(mood);
        }

        // GET: Moods/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mood mood = db.Moods.Find(id);
            if (mood == null)
            {
                return HttpNotFound();
            }
            return View(mood);
        }

        // POST: Moods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Mood mood = db.Moods.Find(id);
            db.Moods.Remove(mood);
            db.SaveChanges();
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
