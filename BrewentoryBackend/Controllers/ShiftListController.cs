using Brewentory.Models;
using BrewentoryBackend.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BrewentoryBackend.Controllers
{
    public class ShiftListController : ApiController
    {
        private int empID;
        // GET: api/ShiftList
        public IEnumerable<string> Get()
        {

            BrewentoryDBEntities1 entities = new BrewentoryDBEntities1();
            string[] shifts = null;

            try
            {
                shifts = (from s in entities.Timesheets select s.EmployeeID + ", " + s.Week + ", " + s.Name + ", " + s.Monday + ", " + s.Tuesday + ", " + s.Wednesday + ", " + s.Thursday + ", " + s.Friday).ToArray();
            }
            finally
            {
                entities.Dispose();
            }

            return shifts;
            
        }
        // <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< CONTINUE HERE. GET DATA TO EDITSHIFTSVIEW FROM HERE >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
        // GET: api/shiftlist/shiftList
        public IEnumerable<string> GetShifts(string shiftList)
        {
            BrewentoryDBEntities1 entities = new BrewentoryDBEntities1();
            string[] shifts = null;

            try
            {
                shifts = (from s in entities.Shifts select  s.ShiftID + ", " + s.ShiftName + ", " + s.ShiftTimes).ToArray();
            }
            finally
            {
                entities.Dispose();
            }
            return shifts;
        }

        
        // GET: api/shiftlist/4
        /*public IEnumerable<string> GetShifts(int id)
        {
            BrewentoryDBEntities1 entities = new BrewentoryDBEntities1();
            string[] shifts = null;

            try
            {
                shifts = (from s in entities.Shifts select s.ShiftName + ", " + s.ShiftTimes).ToArray();
                //var wk = (from w in entities.Timesheets where (w.Week != 0) select w.Week);
                //weekNo = int.Parse(wk.ToString());
            }
            finally
            {
                entities.Dispose();
            }

            return shifts;
        }*/

        // GET: api/shiftlist?employeeID=""
        public BrewentoryModel GetModel(int employeeID)
        {
            BrewentoryDBEntities1 entities = new BrewentoryDBEntities1();

            try
            {
                int empID = employeeID;
                Timesheet timesheet = (from ts in entities.Timesheets where(ts.Name != null) && (ts.EmployeeID == empID) select ts).FirstOrDefault();

                BrewentoryModel chosenEmployeeModel = new BrewentoryModel()
                {
                    EmployeeID = timesheet.EmployeeID,
                    Week = timesheet.Week,
                    Name = timesheet.Name,
                    Monday = timesheet.Monday,
                    Tuesday = timesheet.Tuesday,
                    Wednesday = timesheet.Wednesday,
                    Thursday = timesheet.Thursday,
                    Friday = timesheet.Friday
                };

                return chosenEmployeeModel;
            }
            finally
            {
                entities.Dispose();
            }
        }
        // GET: api/shiftlist?shiftID=""
        public BrewentoryModel GetShiftModel(int shiftID)
        {
            BrewentoryDBEntities1 entities = new BrewentoryDBEntities1();
            try
            {
                Shift shift = (from s in entities.Shifts where (s.ShiftID == shiftID) select s).FirstOrDefault();
                BrewentoryModel chosenShiftModel = new BrewentoryModel()
                {
                    ShiftID = shift.ShiftID,
                    ShiftName = shift.ShiftName,
                    ShiftTimes = shift.ShiftTimes
                };
                return chosenShiftModel;
            }
            finally
            {
                entities.Dispose();
            }
        }

        //   <<<<<<<<<<<<< Actions based on ID. Checked with IF statements >>>>>>>>>>>>>>>
        // GET: api/ShiftList/id
        public List<string[]> GetPickerData(int id) 
        {
            BrewentoryDBEntities1 entities = new BrewentoryDBEntities1();
            List<string[]> data = new List<string[]>(); 
            

            if(id == 5)
            {
                try
                {
                    string[] shifts = (from s in entities.Shifts select s.ShiftName).ToArray();
                    data.Add(shifts);
                }
                finally
                {
                    entities.Dispose();
                }                
            }
            else if(id == 4)
            {
                string[] shifts = (from s in entities.Shifts select s.ShiftTimes).ToArray();
                data.Add(shifts);
                entities.Dispose();                
            }
            
            return data;
        }

        // POST: api/ShiftList
        public bool Post([FromBody]BrewentoryModel model)
        {
            BrewentoryDBEntities1 entities = new BrewentoryDBEntities1();
            try
            {
                if (model.Operation == "Create")
                {
                    Shift shift = (from s in entities.Shifts where (s.ShiftName == model.ShiftName) select s).FirstOrDefault();
                    string shiftTime = shift.ShiftTimes;
                    Timesheet newEntry = new Timesheet()
                    {
                        Week = model.Week,
                        Name = model.Name,                        
                        Monday = shiftTime,
                        Tuesday = shiftTime,
                        Wednesday = shiftTime,
                        Thursday = shiftTime,
                        Friday = shiftTime
                    };

                    entities.Timesheets.Add(newEntry);
                }
                else if(model.Operation == "Edit")
                {
                    Timesheet timesheet = (from t in entities.Timesheets where (t.EmployeeID == model.EmployeeID) select t).FirstOrDefault();
                    if(timesheet != null)
                    {
                        timesheet.Week = model.Week;
                        timesheet.Name = model.Name;
                        if(model.ShiftName == null)
                        {
                            timesheet.Monday = model.Monday;
                            timesheet.Tuesday = model.Tuesday;
                            timesheet.Wednesday = model.Wednesday;
                            timesheet.Thursday = model.Thursday;
                            timesheet.Friday = model.Friday;
                        }
                        else if(model.ShiftName != null)
                        {
                            Shift shift = (from s in entities.Shifts where (s.ShiftName == model.ShiftName) select s).FirstOrDefault();
                            string shiftTime = shift.ShiftTimes;
                            timesheet.Monday = shiftTime;
                            timesheet.Tuesday = shiftTime;
                            timesheet.Wednesday = shiftTime;
                            timesheet.Thursday = shiftTime;
                            timesheet.Friday = shiftTime;
                        }
                        
                    }
                }
                else if(model.Operation == "Delete")
                {
                    Timesheet timesheet = (from t in entities.Timesheets where (t.EmployeeID == model.EmployeeID) select t).FirstOrDefault();
                    if (timesheet != null)
                    {
                        entities.Timesheets.Remove(timesheet);
                    }
                    else return false;
                }

                else if(model.Operation == "EditShift")
                {
                    Shift shift = (from s in entities.Shifts where (s.ShiftID == model.ShiftID) select s).FirstOrDefault();
                    if(shift != null)
                    {
                        shift.ShiftName = model.ShiftName;
                        shift.ShiftTimes = model.ShiftTimes;
                    }
                }
                else if(model.Operation == "DeleteShift")
                {
                    Shift shift = (from s in entities.Shifts where (s.ShiftID == model.ShiftID) select s).FirstOrDefault();
                    if (shift != null) entities.Shifts.Remove(shift);
                }
                else if(model.Operation == "CreateShift")
                {
                    Shift newEntry = new Shift()
                    {
                        ShiftName = model.ShiftName,
                        ShiftTimes = model.ShiftTimes
                    };
                    entities.Shifts.Add(newEntry);
                }
                else if(model.Operation == "AddWeek")
                {
                    var first = (from tw in entities.Timesheets select tw).FirstOrDefault();
                    var prevWk = first.Week;
                    var sheet = (from ts in entities.Timesheets where (ts.Week == prevWk) select ts).ToArray();

                    foreach(var item in sheet)
                    {                       
                        Timesheet newEntry = new Timesheet()
                        {
                            Week = model.Week,
                            Name = item.Name,
                            Monday = item.Monday,
                            Tuesday = item.Tuesday,
                            Wednesday = item.Wednesday,
                            Thursday = item.Thursday,
                            Friday = item.Friday
                        };
                        entities.Timesheets.Add(newEntry);                                               
                    }                  
                }
                else if(model.Operation == "DeleteWeek")
                {
                    var sheet = (from ts in entities.Timesheets where (ts.Week == model.Week) select ts).ToArray();

                    foreach(var item in sheet)
                    {
                        entities.Timesheets.Remove(item);
                    }
                }


                entities.SaveChanges();
            }
            catch
            {
                return false;
            }
            finally
            {
                entities.Dispose();
            }

            return true;
        }

      

        // PUT: api/ShiftList/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/ShiftList/5
        public void Delete(int id)
        {
        }
    }
}
