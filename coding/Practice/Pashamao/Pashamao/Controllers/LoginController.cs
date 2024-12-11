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
            try
            {
                //被踢出去之後, 記下紀錄並清除會話
                if (TempData["KickOutMessage"] != null)
                {
                    ViewBag.Message = TempData["KickOutMessage"].ToString();
                    Session.Clear();
                    Session.Abandon();
                    Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddYears(-1);
                }
                else
                {
                    ViewBag.Message = "";
                }

                //判斷狀態為登入或未登入
                if (Session["UserSeesion"] != null) return RedirectToAction("Index", "MainHome");
                
                return View();
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }
        }

        /// <summary>
        /// 提交登入表單
        /// </summary>
        /// <param name="userViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Submit(LoginUserViewModel userViewModel)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    return View("Index");
                }

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
