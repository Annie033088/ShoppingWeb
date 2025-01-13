using NLog;
using Pashamao.Filters;
using Pashamao.Models;
using Pashamao.Models.Dto.MemberDto;
using Pashamao.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Pashamao.Controllers
{
    [RequestLoggerFilter]
    [UserKickOutFilter]
    [UserRoleAuthFilter(UserPermission.CreateMember | UserPermission.SelectMember | UserPermission.EditMemberPersonalData | UserPermission.EditMemberLevelAndStatus)]
    public class MainMemberController : Controller
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private MainMemberService mainMemberService;

        public MainMemberController()
        {
            mainMemberService = new MainMemberService();
        }

        /// <summary>
        /// 主頁
        /// </summary>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 取得會員(無搜尋狀態)
        /// </summary>
        public ActionResult GetSortedMember(RequestGetSortedMemberDto getSortedMemberDto)
        {
            try
            {
                ErrorCodeDefine errorCode = 0;
                //檢查前端資料
                if (!ModelState.IsValid)
                {
                    errorCode = ErrorCodeDefine.InvalidFormatOrEntry;
                    return Json(new { errorCode });
                }

                (List<Member> members, int totalPage) = mainMemberService.GetSortedMember(getSortedMemberDto);

                if (members == null)
                {
                    errorCode = ErrorCodeDefine.Success;
                    return Json((new { errorCode }));
                }
                else
                {
                    List<ResponseMainMemberDto> memberDtos = members.Select(member => (new ResponseMainMemberDto(member))).ToList();
                    errorCode = ErrorCodeDefine.Success;
                    return Json((new { members = memberDtos, totalPage, errorCode }));
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

        /// <summary>
        /// 搜尋會員並取得會員
        /// </summary>
        [HttpPost]
        public ActionResult SelectMember(RequestGetSelectMemberDto getSelectMemberDto)
        {
            try
            {
                ErrorCodeDefine errorCode = 0;
                //檢查前端資料
                if (!ModelState.IsValid)
                {
                    errorCode = ErrorCodeDefine.InvalidFormatOrEntry;
                    return Json(new { errorCode });
                }

                (List<Member> members, int totalPage) = mainMemberService.SelectMember(getSelectMemberDto);

                if (members == null)
                {
                    errorCode = ErrorCodeDefine.Success;
                    return Json((new { errorCode }));
                }
                else
                {
                    List<ResponseMainMemberDto> memberDtos = members.Select(member => (new ResponseMainMemberDto(member))).ToList();
                    errorCode = ErrorCodeDefine.Success;
                    return Json((new { members = memberDtos, totalPage, errorCode }));
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

        /// <summary>
        /// 修改會員等級跟狀態
        /// </summary>
        [UserRoleAuthFilter(UserPermission.EditMemberLevelAndStatus)]
        [HttpPost]
        public ActionResult SubmitEditMemberlevel(RequestEditMemberLevelAndStatusDto editMemberLevelAndStatusDto)
        {
            try
            {
                ErrorCodeDefine errorCode = 0;
                //檢查前端資料
                if (!ModelState.IsValid)
                {
                    errorCode = ErrorCodeDefine.InvalidFormatOrEntry;
                    return Json(new { errorCode });
                }

                bool successFlag = mainMemberService.EditMemberLevelAndStatus(editMemberLevelAndStatusDto);

                if (successFlag)
                {
                    errorCode = ErrorCodeDefine.Success;
                    return Json(new { errorCode });
                }
                else
                {
                    errorCode = ErrorCodeDefine.DeleteFailed;
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