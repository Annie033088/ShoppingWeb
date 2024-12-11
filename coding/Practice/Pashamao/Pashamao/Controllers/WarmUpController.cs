using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pashamao.Controllers
{
    public class WarmUpController : Controller
    {
        public ActionResult Index()
        {
            return Content("Application Warmed Up!");
        }
    }
}