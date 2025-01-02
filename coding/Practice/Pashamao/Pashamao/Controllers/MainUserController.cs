using Newtonsoft.Json;
using NLog;
using Pashamao.Filters;
using Pashamao.Models;
using Pashamao.Models.Dto.User;
using Pashamao.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace Pashamao.Controllers
{
    [UserKickOutFilter]
    public class MainUserController : Controller
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private MainUserService mainUserService;
        public MainUserController()
        {
            mainUserService = new MainUserService();
        }

        /// <summary>
        /// 後端使用者管理主頁
        /// </summary>
        [UserRoleAuthFilter(UserPermission.CreateUser | UserPermission.DelUser | UserPermission.EditUser | UserPermission.SelectUser)]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 取得排序資料
        /// </summary>
        [HttpPost]
        public ActionResult GetSortedUser(RequestGetSortedUserDto sortedUserDto)
        {
            try
            {
                return Json(mainUserService.GetSortedUser(sortedUserDto), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }
        }

        /// <summary>
        /// 根據欄位查詢排序後使用者
        /// </summary>
        public ActionResult SelectUser(string SelectColumn, string Value, string SortColumn, string Page, string SortOrder)
        {
            try
            {
                (List<User> users, int totalPages) = mainUserService.SelectUser(SelectColumn, Value, SortColumn, Page, SortOrder);

                if (users == null)
                {
                    string noUser = "noUser";
                    return Json(noUser, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json((users, totalPages), JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception e)
            {
                logger.Error(e);
                return View("Index");
                throw e;
            }
        }

        /// <summary>
        /// 創造新的使用者
        /// </summary>
        [UserRoleAuthFilter(UserPermission.CreateUser)]
        public ActionResult GetCreateUserView()
        {
            try
            {
                List<Role> roles = mainUserService.GetRoleIdAndName();
                List<ResponseRoleIdAndNameDto> roleIdAndNameDto = roles.Select(role => (new ResponseRoleIdAndNameDto(role))).ToList();

                ViewBag.Message = TempData["Message"];
                ViewBag.JsonRoles = JsonConvert.SerializeObject(new { roles = roleIdAndNameDto });
                return View("CreateUser");
            }
            catch (Exception e)
            {
                logger.Error(e);
                return View("Index");
                throw e;
            }
        }

        /// <summary>
        /// 提交創建使用者表單
        /// </summary>
        [HttpPost]
        [UserRoleAuthFilter(UserPermission.CreateUser)]
        public ActionResult CreateUser(RequestCreateUserDto createUserDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);

                    foreach (var error in errors)
                    {
                        TempData["Message"] = error.ErrorMessage;
                    }

                    return RedirectToAction("GetCreateUserView");
                }

                bool success = mainUserService.CreateUser(createUserDto);

                if (success)
                {
                    return View("Index");
                }
                else
                {
                    TempData["Message"] = "帳號重複，創建失敗";
                    return RedirectToAction("GetCreateUserView");
                }
            }
            catch (Exception e)
            {
                ViewBag.Message = "創建失敗";
                logger.Error(e);
                return RedirectToAction("GetCreateUserView");
                throw e;
            }
        }

        /// <summary>
        /// 提交修改角色權限
        /// </summary>
        [HttpPost]
        [UserRoleAuthFilter(UserPermission.EditUser)]
        public ActionResult EditUserRoleAndStatus(RequestEditUserRoleAndStatusDto editUserRoleAndStatus)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Json(false);
                }

                bool successFlag = mainUserService.EditUserRoleAndStatus(editUserRoleAndStatus);
                return Json(successFlag);
            }
            catch (Exception e)
            {
                logger.Error(e);
                return View("Index");
                throw e;
            }
        }

        /// <summary>
        /// 刪除使用者
        /// </summary>
        [UserRoleAuthFilter(UserPermission.DelUser)]
        public ActionResult DeleteUser(int UserId)
        {
            try
            {
                bool successFlag = mainUserService.DeleteUser(UserId);
                return Json(successFlag);
            }
            catch (Exception e)
            {
                logger.Error(e);
                return Json(false);
                throw e;
            }
        }

        /// <summary>
        /// 到改密碼頁面
        /// </summary>
        public ActionResult GetEditUserPwdView()
        {
            return View();
        }

        /// <summary>
        /// 提交修改密碼
        /// </summary>
        [HttpPost]
        public ActionResult SubmitEditUserPwd(string OldPwd, string NewPwd)
        {
            try
            {
                if (OldPwd == NewPwd)
                {
                    ViewBag.Message = "密碼輸入重複";
                    return View("EditUserPwd");
                }

                if (mainUserService.EditUserPwd(OldPwd, NewPwd))
                {
                    TempData["Message"] = "修改密碼成功";
                    return RedirectToAction("Index", "MainHome");
                }

                ViewBag.Message = "密碼輸入錯誤";
                return View("EditUserPwd");
            }
            catch (Exception e)
            {
                logger.Error(e);
                return View("Index");
                throw e;
            }
        }
    }
}