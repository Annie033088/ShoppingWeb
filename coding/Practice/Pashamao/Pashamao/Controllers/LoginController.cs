using NLog;
using Pashamao.Models;
using Pashamao.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace Pashamao.Controllers
{
    public class LoginController : Controller
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger ();
        // GET: Login
        public ActionResult Index()
        {
            return View ();
        }

        [HttpPost]
        public ActionResult Submit( UserViewModel userViewModel )
        {
            try
            {
                UserLoginService userLogin = new UserLoginService ();

                if (userLogin.VarifyUser ( userViewModel.LoginAcct, userViewModel.LoginPwd ))
                {
                    logger.Info ( $"User '{userViewModel.LoginAcct}' logged in successfully at {DateTime.Now}." );
                    return RedirectToAction ( "loginSeccess", "ManBackend" );
                } else
                {
                    ViewData["Message"] = $"登入失敗";
                    return View ("Index");
                }
            } catch (Exception e)
            {
                return View ( "Index" );
                throw e;
            }
        }
    }
}
