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
    public class InventoryViewController : Controller
    {

        private BrewentoryDBEntities1 db = new BrewentoryDBEntities1();

        // GET: InventoryView
        [HttpGet]
        public ActionResult Index()
        {
            var inventory = db.Inventories;
            return View(inventory.ToList());
        }

        // Search function
        public ActionResult Index(string searchEntry)
        {
            var inventory = db.Inventories;            
            var list = inventory.Where(i => i.Location.Contains(searchEntry) || i.Product.Contains(searchEntry) || i.Quantity.Contains(searchEntry));            
            return View(list);
        }

        // GET: InventoryView/Details/5
        public ActionResult Details(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Inventory inventory = db.Inventories.Find(id);

            if(inventory == null)
            {
                return HttpNotFound();
            }
            return View(inventory);
        }

        // GET: InventoryView/Create
        public ActionResult Create()
        {
            //ViewBag.InventoryID = new SelectList(db.Inventories, "LocationID", "Location");
            return View();
        }

        // POST: InventoryView/Create
        [HttpPost]
        public ActionResult Create([Bind(Include = "LocationID, Location, Product, Quantity")] Inventory inventory)
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
                db.Inventories.Add(inventory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(inventory);
        }

        // GET: InventoryView/Edit/5
        public ActionResult Edit(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Inventory inventory = db.Inventories.Find(id);

            if(inventory == null)
            {
                return HttpNotFound();
            }

            return View(inventory);
        }

        // POST: InventoryView/Edit/5
        [HttpPost]
        public ActionResult Edit([Bind(Include = "LocationID, Location, Product, Quantity")] Inventory inventory)
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
                db.Entry(inventory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(inventory);
        }

        // GET: InventoryView/Delete/5
        public ActionResult Delete(int? id)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Inventory inventory = db.Inventories.Find(id);

            if(inventory == null)
            {
                return HttpNotFound();
            }

            return View(inventory);
        }

        // POST: InventoryView/Delete/5
        [HttpPost, ActionName("Delete")]
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

            Inventory inventory = db.Inventories.Find(id);
            db.Inventories.Remove(inventory);
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
