using Pashamao.Models;
using Pashamao.Service;
using System;
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

        public ActionResult CreateUser()
        {
            return View ();
        }

        [HttpPost]
        public ActionResult SubmitCreateUser( CreateUserViewModel createUserViewModel )
        {
            try
            {
                manUserService.CreateUser ( createUserViewModel );
                return View ( "Index" );
            } catch (Exception e)
            {
                return View ( "CreateUser" );
                throw e;
            }
        }

        public ActionResult EditUser( string UID, string Status )
        {
            ViewBag.UID = UID;
            ViewBag.Status = Status;
            return View ();
        }

        public ActionResult SubmitEditUser( string UID, string Role, string Status )
        {
            manUserService.EditUser ( UID, Role, Status );
            return View ( "Index" );
        }

        public ActionResult DeleteUser( string UID )
        {
            manUserService.DeleteUser ( UID );
            return View ( "Index" );
        }

        public ActionResult SelectUser( string UID )
        {
            if (manUserService.GetUser ( UID ) == null)
            {
                string noUser = "noUser";
                return Json ( noUser, JsonRequestBehavior.AllowGet );
            } else
            {
                return Json ( manUserService.GetUser ( UID ), JsonRequestBehavior.AllowGet );

            }
        }

    }
}