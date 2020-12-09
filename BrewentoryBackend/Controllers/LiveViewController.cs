using Brewentory.Models;
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
    public class LiveViewController : Controller
    {

        private BrewentoryDBEntities1 db = new BrewentoryDBEntities1();

        // GET: LiveView
        public ActionResult Index()
        {           
            var liveView = db.LiveViews;
            var employees = db.Employees;
            if (employees != null) ViewBag.data = employees;
            return View(liveView.ToList());
        }

        // GET: LiveView/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }


        // We dont use create, but its here just in case we need it at some point in the future. 

        // GET: LiveView/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LiveView/Create
        [HttpPost]
        public ActionResult Create(LiveView liveView)
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

            if (ModelState.IsValid)
            {
                db.LiveViews.Add(liveView);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            
            return View(liveView);
        }

        // GET: LiveView/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LiveView liveView = db.LiveViews.Find(id);
            if (liveView == null)
            {
                return HttpNotFound();
            }

            return View(liveView);
        }

        // POST: LiveView/Edit/5
        [HttpPost]
        public ActionResult Edit(LiveView liveView)
        {
           if(ModelState.IsValid)
           {
                db.Entry(liveView).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
           }
            return View(liveView);
        }

        // GET: LiveView/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LiveView/Delete/5
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


        // GET: Get the work to add as completed
        [HttpGet]
        public ActionResult AddToCompleted()
        {           
            var completedWork = db.CompletedWorks;
            LiveView liveView = db.LiveViews.Find(1);
            CompletedWork newEntry = new CompletedWork()
            {
                Date = DateTime.Now,                
                Product = liveView.Product,
                Batch = liveView.Batch,
                Pallets = liveView.Pallets,
                Quantity = liveView.Quantity
            };           
            return View(newEntry);
        }

        // POST: Add work to completed
        [HttpPost]
        public ActionResult AddToCompleted(CompletedWork completed)
        {
            if(ModelState.IsValid)
            {
                db.CompletedWorks.Add(completed);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(completed);
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
