using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pashamao.Controllers
{
    public class ProductController : Controller
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        public ActionResult Index()
        {
            return View();
        }
    }
}