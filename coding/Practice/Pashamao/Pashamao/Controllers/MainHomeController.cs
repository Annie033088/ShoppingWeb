using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pashamao.Filters;

namespace Pashamao.Controllers
{
    [UserKickOutFilter]
    public class MainHomeController : Controller
    {
        // GET: ManHome
        public ActionResult Index()
        {
            return View();
        }

    }
}