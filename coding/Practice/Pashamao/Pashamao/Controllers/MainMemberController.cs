using NLog;
using Pashamao.Filters;
using Pashamao.Models;
using Pashamao.Models.Dto.MemberDto;
using Pashamao.Models.Dto.UserDto;
using Pashamao.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Pashamao.Controllers
{
    [UserKickOutFilter]
    [UserRoleAuthFilter(UserPermission.CreateMember | UserPermission.SelectMember | UserPermission.EditMemberPersonalData |  UserPermission.EditMemberLevelAndStatus)]
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
                if (!ModelState.IsValid)
                {
                    string errorMessage = "無效的輸入格式";
                    return Json(new { errorMessage });
                }

                (List<Member> members, int totalPage) = mainMemberService.GetSortedMember(getSortedMemberDto);

                if (members == null)
                {
                    string errorMessage = "沒有會員";
                    return Json(new { errorMessage });
                }
                else
                {
                    List<ResponseMainMemberDto> memberDtos = members.Select(member => (new ResponseMainMemberDto(member))).ToList();
                    return Json((new { members = memberDtos, totalPage }));
                }
            }
            catch (Exception e)
            {
                string errorMessage = "發生錯誤，請再試一次";
                logger.Error(e);
                return Json(new { errorMessage });
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
                if (!ModelState.IsValid)
                {
                    string errorMessage = "無效的輸入格式";
                    return Json(new { errorMessage });
                }

                (List<Member> members, int totalPage) = mainMemberService.SelectMember(getSelectMemberDto);

                if (members == null)
                {
                    string errorMessage = "沒有此會員";
                    return Json(new { errorMessage });
                }
                else
                {
                    List<ResponseMainMemberDto> memberDtos = members.Select(member => (new ResponseMainMemberDto(member))).ToList();
                    return Json((new { members = memberDtos, totalPage }));
                }
            }
            catch (Exception e)
            {
                string errorMessage = "發生錯誤，請再試一次";
                logger.Error(e);
                return Json(new { errorMessage });
                throw e;
            }
        }

        /// <summary>
        /// 創建會員頁面
        /// </summary>
        [UserRoleAuthFilter(UserPermission.CreateMember)]
        public ActionResult GetCreateMemberView()
        {
            return View("CreateMember");
        }

        /// <summary>
        /// 提交創建會員表單
        /// </summary>
        [UserRoleAuthFilter(UserPermission.CreateMember)]
        [HttpPost]
        public ActionResult CreateMember(RequestCreateMemberDto createMemberDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ViewBag.Message = "格式輸入";

                    foreach (var key in ModelState.Keys)
                    {
                        var state = ModelState[key];

                        if (state.Errors.Count > 0)
                        {
                            foreach (var error in state.Errors)
                            {
                                ViewBag.Message = ViewBag.Message + "失敗: " + error.ErrorMessage + "；";
                            }
                        }
                    }

                    return View("CreateMember");
                }

                if (createMemberDto.CountryCode != null)
                {
                    if (createMemberDto.Phone == null)
                    {
                        ViewBag.Message = "無效的電話號碼, 請再試一次";
                        return View("CreateMember");
                    }
                }

                bool successFlag = mainMemberService.CreateMember(createMemberDto);

                if (successFlag)
                {
                    return View("Index");
                }
                else
                {
                    ViewBag.Message = "帳號重複, 創建失敗, 請再試一次";
                    return View("CreateMember");
                }
            }
            catch (Exception e)
            {
                ViewBag.Message = "創建失敗, 請再試一次";
                logger.Error(e);
                return View("CreateMember");
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
                if (!ModelState.IsValid)
                {
                    string errorMessage = "無效的輸入格式";
                    return Json(new { errorMessage });
                }

                bool successFlag = mainMemberService.EditMemberLevelAndStatus(editMemberLevelAndStatusDto);

                if (successFlag)
                {
                    return Json(new { successFlag });
                }
                else
                {
                    string errorMessage = "刪除失敗";
                    return Json(new { errorMessage });
                }
            }
            catch (Exception e)
            {
                string errorMessage = "發生錯誤，請再試一次";
                logger.Error(e);
                return Json(new { errorMessage });
                throw e;
                throw;
            }
        }
    }
}