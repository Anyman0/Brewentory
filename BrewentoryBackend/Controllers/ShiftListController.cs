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
        private int weekNo;
        // GET: api/ShiftList
        public IEnumerable<string> Get()
        {
            BrewentoryDBEntities1 entities = new BrewentoryDBEntities1();
            string[] shifts = null;

            try
            {
                shifts = (from s in entities.Shifts select s.ShiftName + ", " + s.ShiftTimes).ToArray();
                var wk = (from w in entities.Timesheets where (w.Week != 0) select w.Week);
                weekNo = int.Parse(wk.ToString());
            }
            finally
            {
                entities.Dispose();
            }

            return shifts;
        }

        // GET: api/ShiftList/5
        public List<string[]> GetPickerData(int id) 
        {
            BrewentoryDBEntities1 entities = new BrewentoryDBEntities1();
            List<string[]> pickerData = new List<string[]>();

            try
            {
                string[] shifts = (from s in entities.Shifts select s.ShiftName).ToArray();
                pickerData.Add(shifts);
            }
            finally
            {
                entities.Dispose();
            }
            return pickerData;
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
                        Week = weekNo,
                        Name = model.Name,                        
                        Monday = shiftTime,
                        Tuesday = shiftTime,
                        Wednesday = shiftTime,
                        Thursday = shiftTime,
                        Friday = shiftTime
                    };

                    entities.Timesheets.Add(newEntry);
                }

                else if(model.Operation == "Delete")
                {
                    Timesheet ts = (from t in entities.Timesheets where (t.Name == model.Name) select t).FirstOrDefault();
                    if(ts == null)
                    {
                        return false;
                    }
                    int timesheetId = ts.EmployeeID;
                    Timesheet existing = (from e in entities.Timesheets where (e.EmployeeID == timesheetId) select e).FirstOrDefault();
                    if(existing != null)
                    {
                        entities.Timesheets.Remove(existing);
                    }
                    else
                    {
                        return false;
                    }
                }

                entities.SaveChanges();
            }
            catch
            {

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
