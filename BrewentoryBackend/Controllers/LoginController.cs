using Brewentory.Models;
using BrewentoryBackend.DataAccess;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.ServiceModel.Channels;
using System.Web;
using System.Web.Mvc;

namespace BrewentoryBackend.Controllers
{
    public class LoginController : Controller
    {
        private BrewentoryDBEntities1 db = new BrewentoryDBEntities1();       
        private int id;       
        
        // GET: Login
        [HttpGet]
        public ActionResult Index()
        {                        
            var employees = db.Employees;            
            foreach(var employee in employees)
            {
                if (employee.LoggedIn == true)
                {
                    ViewData["testMessage"] = "Welcome, " + employee.Name;
                }
                else ViewData["testMessage"] = "Login with your credentials.";
            }
            if (employees != null) ViewBag.data = employees;
            return View(db.Employees.ToList());            
        }
        

        [HttpPost]
        public ActionResult Index(string Employee)
        {
            try
            {
                BrewentoryModel model = new BrewentoryModel();
                var name = Employee;
                foreach (var e in db.Employees)
                {
                    if (e.Name == name)
                    {
                        id = e.EmployeeID;
                    }
                }
                Employee emp = db.Employees.Find(id);
                if (MatchSHA1(emp.Password, GetSHA1(emp.EmployeeID.ToString(), Request["enteredPassword"])))
                {
                    if (emp.LoggedIn == false)
                        emp.LoggedIn = true;
                    model.User = emp.Name;
                    
                }
                else if (!MatchSHA1(emp.Password, GetSHA1(emp.EmployeeID.ToString(), Request["enteredPassword"])))
                {
                    if (emp.LoggedIn == true)
                        emp.LoggedIn = false;
                    model.User = "Login";
                }

                

                if (ModelState.IsValid)
                {
                    db.Entry(emp).State = EntityState.Modified;
                    db.SaveChanges();
                }

            }
            catch
            {

            }
                                
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Login(string Employee) 
        {
            var name = Employee;           
            foreach(var e in db.Employees)
            {
                if(e.Name == name)
                {
                    id = e.EmployeeID;
                }
            }            
            Employee emp = db.Employees.Find(id);           
            if (MatchSHA1(emp.Password, GetSHA1(emp.EmployeeID.ToString(), Request["enteredPassword"])))
            {
                // Save LoggedInAsAdmin - status somewhere. 
                ViewBag.Data = "Welcome " + emp.Name + "!";
            }
            else if (!MatchSHA1(emp.Password, GetSHA1(emp.EmployeeID.ToString(), Request["enteredPassword"])))
            {
                ViewBag.Data = "Failed! Password was wrong.";
            }

            return RedirectToRoute("/LiveView/");       
        }

        
        /// <summary>
        /// If the two SHA1 hashes are the same, returns true.
        /// Otherwise returns false.
        /// </summary>
        /// <param name="p1"></param>
        /// <param name="p2"></param>
        /// <returns></returns>
        private static bool MatchSHA1(byte[] p1, byte[] p2)
        {
            bool result = false;
            if (p1 != null && p2 != null)
            {
                if (p1.Length == p2.Length)
                {
                    result = true;
                    for (int i = 0; i < p1.Length; i++)
                    {
                        if (p1[i] != p2[i])
                        {
                            result = false;
                            break;
                        }
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// Returns the SHA1 hash of the combined userID and password.
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private static byte[] GetSHA1(string userID, string password)
        {
            SHA1CryptoServiceProvider sha = new SHA1CryptoServiceProvider();
            return sha.ComputeHash(System.Text.Encoding.ASCII.GetBytes(userID + password));
        }
    }
}