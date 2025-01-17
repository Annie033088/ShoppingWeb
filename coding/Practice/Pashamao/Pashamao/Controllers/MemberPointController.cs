using NLog;
using Pashamao.Filters;
using Pashamao.Models;
using Pashamao.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pashamao.Controllers
{
    [RequestLoggerFilter]
    [UserKickOutFilter]
    [UserRoleAuthFilter(UserPermission.ResetMemberPoints)]
    public class MemberPointController : Controller
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private MemberPointService memberPointService;

        public MemberPointController()
        {
            memberPointService = new MemberPointService();
        }
        // GET: MemberPoint
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ResetMemberPoint()
        {
            try
            {
                ErrorCodeDefine errorCode = 0;
                bool successFlag = memberPointService.ResetMemberPoint();

                if (successFlag)
                {
                    errorCode = ErrorCodeDefine.Success;
                    return Json(new { errorCode });
                }
                else
                {
                    errorCode = ErrorCodeDefine.ModifiedFailed;
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