using Pashamao.Repositories;
using Pashamao.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pashamao.Controllers
{
    public class ManUserController : Controller
    {
        private ManUserService manUserService;
        public ManUserController()
        {
            manUserService = new ManUserService ();
        }
        // GET: ManUser
        public ActionResult Index()
        {
            return View ();
        }

        
        public ActionResult GetAllUser()
        {
            return Json ( manUserService.GetAllUsers (), JsonRequestBehavior.AllowGet );
        }

        [HttpPost]
        public ActionResult CreateUser()
        {

            return View ();
        }

    }
}