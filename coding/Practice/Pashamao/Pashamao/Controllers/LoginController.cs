using NLog;
using Pashamao.Models;
using Pashamao.Service;
using System;
using System.Web;
using System.Web.Mvc;

namespace Pashamao.Controllers
{
    public class LoginController : Controller
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// 登入主頁
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 提交登入表單
        /// </summary>
        /// <param name="userViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Submit(UserViewModel userViewModel)
        {
            try
            {
                UserLoginService userLogin = new UserLoginService();

                if (userLogin.VerifyAndGetUser(userViewModel.LoginAcct, userViewModel.LoginPwd))
                {
                    //禁用的帳號?
                    if (userLogin.AcctSuspended())
                    {
                        ViewData["Message"] = $"你的帳號被禁用了";
                        return View("Index");
                    }

                    //用cookie傳出個人化資料
                    HttpCookie cookie = new HttpCookie("cookieName", userLogin.GetUserName())
                    {
                        Secure = true
                    };
                    Response.Cookies.Add(cookie);

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
