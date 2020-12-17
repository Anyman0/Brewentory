using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace Brewentory.Models
{
   public class LoginModel

   {    [PrimaryKey, AutoIncrement]  
        public int ID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }       
   }
}
