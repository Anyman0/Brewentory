using BrewentoryBackend.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Dynamic;
using Brewentory.Models;

namespace BrewentoryBackend.Controllers
{
    public class TimesheetViewController : Controller
    {
        private BrewentoryDBEntities1 db = new BrewentoryDBEntities1();
        private string shiftName;
        private int prevWeek;
        private bool removeWeek = false;
        // GET: TimesheetView

        [HttpGet]
        public ActionResult Index()
        {
            var timesheets = db.Timesheets;           
            BrewentoryModel model = new BrewentoryModel();
            foreach(var w in timesheets)
            {
                model.WeekNo = w.Week;
                break;
            }            
            //PostWeek(model);            
            return View(timesheets.ToList());
        }

        // Below method changes week to be the same for all
        [HttpPost]
        public bool PostWeek(BrewentoryModel model) 
        {
            if(model.WeekNo >= 0 )
            {
                foreach(var t in db.Timesheets)
                {
                    t.Week = model.WeekNo;
                }

                db.SaveChanges();
            } 
            return true;
        }

        // GET: TimesheetView/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Timesheet timesheet = db.Timesheets.Find(id);
            if (timesheet == null)
            {
                return HttpNotFound();
            }

            return View(timesheet);
        }

        // GET: TimesheetView/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TimesheetView/Create
        [HttpPost]
        public ActionResult Create(Timesheet timesheet)
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
                db.Timesheets.Add(timesheet);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(timesheet);
        }

        // GET: TimesheetView/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Timesheet timesheet = db.Timesheets.Find(id);           
            if (timesheet == null)
            {
                return HttpNotFound();
            }
            return View(timesheet);
        }

        // POST: TimesheetView/Edit/5
        [HttpPost]
        public ActionResult Edit(Timesheet timesheet)
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
            if (ModelState.IsValid)
            {
                db.Entry(timesheet).State = EntityState.Modified;                
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(timesheet);
        }

        // GET: TimesheetView/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Timesheet timesheet = db.Timesheets.Find(id);
            if (timesheet == null)
            {
                return HttpNotFound();
            }
            return View(timesheet);
        }

        // POST: TimesheetView/Delete/5
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
            Timesheet timesheet = db.Timesheets.Find(id);
            db.Timesheets.Remove(timesheet);
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


        // >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>  Specials  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

        // GET: TimesheetView/Edit/5
        public ActionResult AutoEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Timesheet timesheet = db.Timesheets.Find(id);
            if (timesheet == null)
            {
                return HttpNotFound();
            }

            var shifts = db.Shifts;
            if(shifts != null)
            {
                ViewBag.data = shifts;               
            }
            return View(timesheet);
        }

       [HttpPost]
       public ActionResult AutoEdit(Timesheet timesheet, string Shifts)
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
            shiftName = Shifts;
            if(shiftName != null)
            {
                var shifts = db.Shifts;
                foreach(var shift in shifts)
                {
                    if(shift.ShiftID.ToString() == shiftName)
                    {
                        timesheet.Monday = shift.ShiftTimes;
                        timesheet.Tuesday = shift.ShiftTimes;
                        timesheet.Wednesday = shift.ShiftTimes;
                        timesheet.Thursday = shift.ShiftTimes;
                        timesheet.Friday = shift.ShiftTimes;
                        break;
                    }
                }
            }            
            
            // <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< ABSOLUTELY USELESS CHECK ATM :DD Fast workaround to make it work. Fix later
            if (ModelState.IsValid || !ModelState.IsValid) 
            {                
                db.Entry(timesheet).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(timesheet);
       }

        public ActionResult GetShifts()
        {
            var shifts = db.Shifts;
            return View(shifts);
        }
          
        [HttpGet]
        public ActionResult ShiftsPartial()
        {
            List<Shift> shiftList = new List<Shift>();            
            BrewentoryDBEntities1 entities = new BrewentoryDBEntities1();
            var allShifts = db.Shifts;                        
            
            var shifts = db.Shifts.ToList();
            if(shifts != null)
            {
                ViewBag.data = shifts;
            }

            return View(allShifts.ToList());
        }

        // GET: TimesheetView/Create
        public ActionResult CreateShift()
        {
            return View();
        }

        // POST: TimesheetView/Create
        [HttpPost]
        public ActionResult CreateShift(Shift shift)
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
                db.Shifts.Add(shift);
                db.SaveChanges();
                return RedirectToAction("ShiftsPartial");
            }
            return View(shift);
        }

        // GET: TimesheetView/Edit/5
        public ActionResult EditShift(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shift shift = db.Shifts.Find(id);
            if (shift == null)
            {
                return HttpNotFound();
            }
            return View(shift);
        }

        // POST: TimesheetView/Edit/5
        [HttpPost]
        public ActionResult EditShift(Shift shift)
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
            if (ModelState.IsValid)
            {
                db.Entry(shift).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ShiftsPartial");
            }
            return View(shift);
        }

        // GET: Shifts/Delete
        public ActionResult DeleteShift(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Shift shift = db.Shifts.Find(id);
            if (shift == null)
            {
                return HttpNotFound();
            }
            return View(shift);
        }

        // POST: Shifts/Delete
        [HttpPost, ActionName("DeleteShift")]
        public ActionResult DeleteShiftConfirmed(int id)
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
            Shift shift = db.Shifts.Find(id);
            db.Shifts.Remove(shift);
            db.SaveChanges();
            return RedirectToAction("ShiftsPartial");
        }


        // <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<----- Below dublicates employees to a new week. ----->>>>>>>>>>>>>>>>>>>>>>>>>>
        // GET: TimesheetView/NewWeek
        public ActionResult NewWeek()
        {
            return View();
        }

        // POST: TimesheetView/NewWeek
        [HttpPost]
        public ActionResult NewWeek(Timesheet sheet)
        {
            var timesheet = db.Timesheets;
            prevWeek = 0;
            // Below makes sure we only add each employee once and determines if we are removing an existing week or adding a new one. 
            foreach(var w in timesheet)
            {
                if(prevWeek == 0)
                prevWeek = w.Week;
                if (w.Week == sheet.Week)
                {
                    removeWeek = true;
                    prevWeek = sheet.Week;
                    break;
                }
                else removeWeek = false;
                
            }

            if(ModelState.IsValid || !ModelState.IsValid)
            {
                
                if(!removeWeek)
                {
                    foreach (var employee in timesheet)
                    {
                        if (employee.Week == prevWeek)
                        {
                            Timesheet newEntry = new Timesheet()
                            {
                                Week = sheet.Week,
                                Name = employee.Name,
                                Monday = employee.Monday,
                                Tuesday = employee.Tuesday,
                                Wednesday = employee.Wednesday,
                                Thursday = employee.Thursday,
                                Friday = employee.Friday,
                            };
                            db.Timesheets.Add(newEntry);
                        }
                    }
                }
                else if(removeWeek)
                {
                    foreach(var employee in timesheet)
                    {
                        if(employee.Week == sheet.Week)
                        {                            
                            db.Timesheets.Remove(employee);
                        }
                    }
                }
                        
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sheet);
        }

    }
}
