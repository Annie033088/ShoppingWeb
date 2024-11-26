using NLog;
using Pashamao.Models;
using Pashamao.Service;
using System;
using System.Web.Mvc;

namespace Pashamao.Controllers
{
    public class LoginController : Controller
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Submit(UserViewModel userViewModel)
        {
            try
            {
                UserLoginService userLogin = new UserLoginService();

                if (userLogin.VarifyUser(userViewModel.LoginAcct, userViewModel.LoginPwd))
                {
                    //禁用的帳號?
                    if (userLogin.AcctSuspended()) return RedirectToAction("SuspendedInfo", "Error");


                    logger.Info($"User '{userViewModel.LoginAcct}' logged in successfully at {DateTime.Now}.");
                    return RedirectToAction("Index", "ManHome");
                }
                else
                {
                    ViewData["Message"] = $"登入失敗";
                    return View("Index");
                }
            }
            catch (Exception e)
            {
                ViewData["Message"] = $"登入失敗, 再試一次";
                return View("Index");
                throw e;
            }

        }
    }
}
