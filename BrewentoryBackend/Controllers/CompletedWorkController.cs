using BrewentoryBackend.DataAccess;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace BrewentoryBackend.Controllers
{
    public class CompletedWorkController : Controller
    {
        private BrewentoryDBEntities1 db = new BrewentoryDBEntities1();
        // GET: CompletedWork
        public ActionResult Index()
        {
            var completedWork = db.CompletedWorks;
            return View(completedWork.ToList());
        }

        
        // GET: CompletedWork/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CompletedWork/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: CompletedWork/Edit/5
        public ActionResult Edit(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompletedWork completed = db.CompletedWorks.Find(id);
            if(completed == null)
            {
                return HttpNotFound();
            }
                                
            return View(completed);
        }

        // POST: CompletedWork/Edit/5
        [HttpPost]
        public ActionResult Edit(CompletedWork cw)
        {
            if(ModelState.IsValid)
            {
                db.Entry(cw).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(cw);
        }

        // GET: CompletedWork/Delete/5
        public ActionResult Delete(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CompletedWork cw = db.CompletedWorks.Find(id);

            if(cw == null)
            {
                return HttpNotFound();
            }

            return View(cw);
        }

        // POST: CompletedWork/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            CompletedWork cw = db.CompletedWorks.Find(id);
            db.CompletedWorks.Remove(cw);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        // <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< SPECIALS >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
        [HttpGet]
        public ActionResult DeleteAll(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }            
            
            return View();
        }

        // Clear the whole list 
        [HttpPost]
        public ActionResult DeleteAll()
        {
            var completedWorks = db.CompletedWorks;
            foreach(var work in completedWorks)
            {
                CompletedWork cw = db.CompletedWorks.Find(work.CompletedWorkID);
                db.CompletedWorks.Remove(cw);
            }
           
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
