using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BrewentoryBackend
{
    public abstract class ViewModel
    {
        public string User { get; set; }
        public string Message { get; set; }
    }

    public class HomeViewModel : ViewModel
    {

    }
}