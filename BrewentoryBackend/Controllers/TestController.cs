using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BrewentoryBackend.Controllers
{
    public class TestController : ApiController
    {
        public string[] GetAll()
        {
            return new string[] { "Test", "Test1" };
        }
    }
}
