using NLog;
using Pashamao.Models;
using Pashamao.Models.Dto.UserLoginDto;
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
        public ActionResult Index()
        {
            try
            {
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
        [HttpPost]
        public ActionResult SubmitUserLogin(RequestLoginUserDto loginUserDto)
        {

            try
            {
                int errorCode = 0;

                if (!ModelState.IsValid)
                {
                    errorCode = 5;
                    return Json(new { errorCode });
                }

                UserLoginService userLogin = new UserLoginService();

                if (userLogin.VerifyAndGetUser(loginUserDto))
                {
                    //禁用的帳號?
                    if (userLogin.AcctSuspended())
                    {
                        errorCode = 3;
                        return Json(new { errorCode });
                    }

                    //用cookie傳出個人化資料
                    HttpCookie cookie = new HttpCookie("cookieName", userLogin.GetUserName())
                    {
                        Secure = true
                    };

                    Response.Cookies.Add(cookie);
                    logger.Info($"User '{loginUserDto.Account}' logged in successfully at {DateTime.Now}.");
                    errorCode = 1;
                    return Json(new { errorCode });
                }
                else
                {
                    errorCode = 9;
                    return Json(new { errorCode });
                }

            }
            catch (Exception e)
            {
                logger.Error(e);
                int errorCode = 6;
                return Json(new { errorCode });
                throw e;
            }
        }
    }
}
