using Pashamao.Models;
using Pashamao.ServerCs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace Pashamao.Controllers
{
    public class LoginController : Controller
    {
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
                PwdHash pwdHash = new PwdHash ();

                if (pwdHash.VarifyPwd ( userViewModel.LoginAcct, userViewModel.LoginPwd ))
                {
                    return RedirectToAction ( "index", "ManBackend" );
                } else
                {
                    ViewData["Message"] = $"登入失敗";
                    return View ("Index");
                }
            } catch (Exception e)
            {
                throw e;
            }
        }
    }
}
