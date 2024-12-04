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
            //被踢出去之後, 記下紀錄並清除會話
            if (Session["KickOutMessage"] != null)
            {
                ViewBag.Message = Session["KickOutMessage"].ToString();
                Session.Clear();
                Session.Abandon();
                Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddYears(-1);
            }

            if (Session["UserVisitState"] != null)
            {
                if (Session["UserVisitState"].ToString() != "Guest")
                    return RedirectToAction("Index", "MainHome");
            }

            Session["UserVisitState"] = "Guest";
            return View();
        }

        /// <summary>
        /// 提交登入表單
        /// </summary>
        /// <param name="userViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Submit(LoginUserViewModel userViewModel)
        {

            if (!ModelState.IsValid)
            {
                return View("Index");
            }

            try
            {
                UserLoginService userLogin = new UserLoginService();

                if (userLogin.VerifyAndGetUser(userViewModel.LoginAcct, userViewModel.LoginPwd))
                {
                    //禁用的帳號?
                    if (userLogin.AcctSuspended())
                    {
                        ViewBag.Message = $"你的帳號被禁用了";
                        return View("Index");
                    }

                    //用cookie傳出個人化資料
                    HttpCookie cookie = new HttpCookie("cookieName", userLogin.GetUserName())
                    {
                        Secure = true
                    };
                    Response.Cookies.Add(cookie);
                    Session["UserVisitState"] = "User";
                    logger.Info($"User '{userViewModel.LoginAcct}' logged in successfully at {DateTime.Now}.");
                    return RedirectToAction("Index", "MainHome");
                }
                else
                {
                    ViewBag.Message = $"登入失敗";
                    return View("Index");
                }

            }
            catch (Exception e)
            {
                ViewBag.Message = $"登入失敗, 再試一次";
                return View("Index");
                throw e;
            }

        }
    }
}
