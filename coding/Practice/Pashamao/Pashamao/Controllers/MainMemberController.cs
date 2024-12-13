using NLog;
using Pashamao.Filters;
using Pashamao.Models;
using Pashamao.Service;
using System.Data.Common;
using System.Data.SqlClient;
using System;
using System.Web.Mvc;
using System.Web.UI;
using System.Collections.Generic;
using System.Diagnostics;

namespace Pashamao.Controllers
{
    [UserKickOutFilter]
    [UserRoleAuthFilter(UserPermission.CreateMember | UserPermission.SelectMember | UserPermission.EditMemberAddress | UserPermission.EditMemberPhone | UserPermission.EditMemberEmail | UserPermission.EditMemberLevel)]
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
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 取得會員(無搜尋狀態)
        /// </summary>
        /// <param name="Column"></param>
        /// <param name="Page"></param>
        /// <param name="SortOrder"></param>
        /// <returns></returns>
        public ActionResult GetSortedMember(string Column, string Page, string SortOrder)
        {
            try
            {
                (List<Member> members, int totalPages) = mainMemberService.GetSortedMember(Column, Page, SortOrder);

                if (members == null)
                {
                    string noMember = "noMember";
                    return Json(noMember, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json((members, totalPages), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }
        }

        /// <summary>
        /// 搜尋會員並取得會員
        /// </summary>
        /// <param name="SelectColumn"></param>
        /// <param name="Value"></param>
        /// <param name="SortColumn"></param>
        /// <param name="Page"></param>
        /// <param name="SortOrder"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SelectMember(string SelectColumn, string Value, string SortColumn, string Page, string SortOrder)
        {
            try
            {
                (List<Member> members, int totalPages) = mainMemberService.SelectMember(SelectColumn, Value, SortColumn, Page, SortOrder);

                if (members == null)
                {
                    string noMember = "noMember";
                    return Json(noMember, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json((members, totalPages), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }
        }

        /// <summary>
        /// 創建會員頁面
        /// </summary>
        /// <returns></returns>
        [UserRoleAuthFilter(UserPermission.CreateMember)]
        public ActionResult CreateMember()
        {
            return View();
        }

        /// <summary>
        /// 提交創建會員表單
        /// </summary>
        /// <param name="createMemberViewModel"></param>
        /// <returns></returns>
        [UserRoleAuthFilter(UserPermission.CreateMember)]
        [HttpPost]
        public ActionResult SubmitCreateMember(CreateMemberViewModel createMemberViewModel)
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

                if (createMemberViewModel.CountryCode != null)
                {
                    if (createMemberViewModel.Phone == null)
                    {
                        ViewBag.Message = "無效的電話號碼, 請再試一次";
                        return View("CreateMember");
                    }
                }

                bool createSuccessFlag = mainMemberService.CreateMember(createMemberViewModel);

                if (createSuccessFlag)
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
                logger.Error(e);
                throw e;
            }
        }

        /// <summary>
        /// 修改會員等級跟狀態
        /// </summary>
        /// <param name="MemberId"></param>
        /// <param name="Level"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        [UserRoleAuthFilter(UserPermission.EditMemberLevel)]
        [HttpPost]
        public ActionResult SubmitEditMemberlevel(string MemberId, string Level, string Status)
        {
            Console.WriteLine(MemberId, Level, Status);
            return Json(mainMemberService.EditMemberlevel(MemberId, Level, Status), JsonRequestBehavior.AllowGet);
        }
    }
}