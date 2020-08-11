using BrewentoryBackend.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace BrewentoryBackend.Controllers
{
    public class NoteViewController : Controller
    {

        private BrewentoryDBEntities1 db = new BrewentoryDBEntities1();
        // GET: NoteView
        public ActionResult Index()
        {
            var notes = db.Notetables;
            return View(notes.ToList());
        }

        // GET: NoteView/Details/5
        public ActionResult Details(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Notetable notes = db.Notetables.Find(id);
            if(notes == null)
            {
                return HttpNotFound();
            }

            return View(notes);
        }

        // GET: NoteView/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: NoteView/Create
        [HttpPost]
        public ActionResult Create(Notetable notetable)
        {
            /*try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }*/
            if(ModelState.IsValid)
            {
                db.Notetables.Add(notetable);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(notetable);
        }

        // GET: NoteView/Edit/5
        public ActionResult Edit(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Notetable notes = db.Notetables.Find(id);
            if(notes == null)
            {
                return HttpNotFound();
            }
            return View(notes);
        }

        // POST: NoteView/Edit/5
        [HttpPost]
        public ActionResult Edit(Notetable notetable)
        {
            /*try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }*/
            if(ModelState.IsValid)
            {
                db.Entry(notetable).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(notetable);
        }

        // GET: NoteView/Delete/5
        public ActionResult Delete(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Notetable note = db.Notetables.Find(id);
            if(note == null)
            {
                return HttpNotFound();
            }
            return View(note);
        }

        // POST: NoteView/Delete/5
        [HttpPost]
        public ActionResult DeleteConfirmed(int id)
        {
            /*try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }*/
            Notetable note = db.Notetables.Find(id);
            db.Notetables.Remove(note);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if(disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
