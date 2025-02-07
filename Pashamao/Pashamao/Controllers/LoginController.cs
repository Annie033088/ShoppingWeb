using NLog;
using Pashamao.Filters;
using Pashamao.Models;
using Pashamao.Models.Dto.UserLoginDto;
using Pashamao.Service;
using System;
using System.Web;
using System.Web.Mvc;

namespace Pashamao.Controllers
{
    [RequestLoggerFilter]
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
                ErrorCodeDefine errorCode = 0;

                if (!ModelState.IsValid)
                {
                    errorCode = ErrorCodeDefine.InvalidFormatOrEntry;
                    return Json(new { errorCode });
                }

                UserLoginService userLogin = new UserLoginService();

                if (userLogin.VerifyAndGetUser(loginUserDto))
                {
                    //禁用的帳號?
                    if (userLogin.AcctSuspended())
                    {
                        errorCode = ErrorCodeDefine.Baned;
                        return Json(new { errorCode });
                    }

                    //用cookie傳出個人化資料
                    HttpCookie cookie = new HttpCookie("cookieName", userLogin.GetUserName())
                    {
                        Secure = true
                    };

                    Response.Cookies.Add(cookie);
                    errorCode = ErrorCodeDefine.Success;
                    return Json(new { errorCode });
                }
                else
                {
                    errorCode = ErrorCodeDefine.LoginFailed;
                    return Json(new { errorCode });
                }
            }
            catch (Exception e)
            {
                ErrorCodeDefine errorCode = ErrorCodeDefine.ServerError;
                logger.Error(e);
                return Json(new { errorCode });
                throw e;
            }
        }
    }
}
