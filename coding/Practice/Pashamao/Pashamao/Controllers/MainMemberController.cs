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
    [UserRoleAuthFilter(UserPermission.CreateMember | UserPermission.SelectMember | UserPermission.EditMemberAddress | UserPermission.EditMemberPhone | UserPermission.EditMemberEmail | UserPermission.EditMemberLevel)]
    [UserKickOutFilter]
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
                                ViewBag.Message = ViewBag.Message + "失敗: " + error.ErrorMessage;
                            }
                        }
                    }

                    return View("CreateMember");
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

        [HttpPost]
        public ActionResult SelectMember(string Column, string Value)
        {
            return View();
        }

    }
}