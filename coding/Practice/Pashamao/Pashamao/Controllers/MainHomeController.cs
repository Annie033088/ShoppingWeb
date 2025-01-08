using NLog;
using Pashamao.Filters;
using Pashamao.Models.Dto.UserDto;
using Pashamao.Service;
using System;
using System.Web.Mvc;

namespace Pashamao.Controllers
{
    [UserKickOutFilter]
    public class MainHomeController : Controller
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private MainUserService mainUserService;

        public MainHomeController()
        {
            mainUserService = new MainUserService();
        }

        /// <summary>
        /// 主頁
        /// </summary>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 登出
        /// </summary>
        public ActionResult Logout()
        {
            try
            {
                Session.Clear();
                Session.Abandon();
                Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddYears(-1);
                //登出的errorCode
                int errorCode = 1;
                return Json((new { errorCode }));
            }
            catch (Exception e)
            {
                logger.Error(e);
                int errorCode = 6;
                return Json(new { errorCode });
                throw e;
            }
        }

        /// <summary>
        /// 到改密碼頁面
        /// </summary>
        public ActionResult GetEditUserPwdView()
        {
            return View("EditUserPwd");
        }

        /// <summary>
        /// 提交修改密碼
        /// </summary>
        [HttpPost]
        public ActionResult SubmitEditUserPwd(RequestEditUserPwdDto editUserPwdDto)
        {
            try
            {
                int errorCode = 0;

                if (!ModelState.IsValid || editUserPwdDto.OldPwd == editUserPwdDto.NewPwd)
                {
                    errorCode = 5;
                    return Json(new { errorCode });
                }

                if (mainUserService.EditUserPwd(editUserPwdDto))
                {
                    errorCode = 1;
                    return Json(new { errorCode });
                }

                errorCode = 10;
                return Json(new { errorCode });
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