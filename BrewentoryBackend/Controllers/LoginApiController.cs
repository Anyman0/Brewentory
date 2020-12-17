using Brewentory.Models;
using BrewentoryBackend.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Web.Http;

namespace BrewentoryBackend.Controllers
{
    public class LoginApiController : ApiController
    {
        private int id;

        [HttpGet]
        public string[] GetAllEmployees()
        {
            string[] employees = null;
            BrewentoryDBEntities1 entities = new BrewentoryDBEntities1();

            try
            {
                employees = (from e in entities.Employees select e.EmployeeID + ", " + e.Name + ", " + e.LoggedIn).ToArray();
            }
            finally
            {
                entities.Dispose();
            }

            return employees;
        }

        [HttpGet]
        [HttpPost]

        public bool GetLogin(string LogData)
        {
            BrewentoryDBEntities1 entities = new BrewentoryDBEntities1();

            string[] logDataParts = LogData.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            string usrname = logDataParts[0];
            string pw = logDataParts[1];

            try
            {
                BrewentoryModel model = new BrewentoryModel();
                var name = usrname;
                foreach (var e in entities.Employees)
                {
                    if (e.Name == name)
                    {
                        id = e.EmployeeID;
                    }
                }
                var emp = (from e in entities.Employees where (e.EmployeeID == id) select e).FirstOrDefault();
                if (MatchSHA1(emp.Password, GetSHA1(emp.EmployeeID.ToString(), pw)))
                {
                    if (emp.LoggedIn == false)
                        emp.LoggedIn = true;
                    return true;

                }
                else if (!MatchSHA1(emp.Password, GetSHA1(emp.EmployeeID.ToString(), pw)))
                {
                    if (emp.LoggedIn == true)
                        emp.LoggedIn = false;                    
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

            return false;

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
