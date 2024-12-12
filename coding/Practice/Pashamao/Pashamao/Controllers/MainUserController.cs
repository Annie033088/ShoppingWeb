using Newtonsoft.Json;
using NLog;
using Pashamao.Filters;
using Pashamao.Models;
using Pashamao.Service;
using System;
using System.Web.Mvc;

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

        [UserRoleAuthFilter(UserPermission.CreateUser | UserPermission.DelUser | UserPermission.EditUser | UserPermission.SelectUser)]
        /// <summary>
        /// 後端使用者管理主頁
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 取得排序資料
        /// </summary>
        /// <param name="Column"></param>
        /// <param name="Page"></param>
        /// <param name="SortOrder"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetSortedUser(string Column, string Page, string SortOrder)
        {
            try
            {
                return Json(mainUserService.GetSortedUser(Column, Page, SortOrder), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }
        }

        /// <summary>
        /// 創造新的使用者
        /// </summary>
        /// <returns></returns>
        [UserRoleAuthFilter(UserPermission.CreateUser)]
        public ActionResult CreateUser()
        {
            try
            {
                ViewBag.Message = TempData["Message"];
                ViewBag.JsonRolesName = JsonConvert.SerializeObject(mainUserService.GetAllRoleName());
                return View();
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
        /// <param name="createUserViewModel"></param>
        /// <returns></returns>
        [HttpPost]
        [UserRoleAuthFilter(UserPermission.CreateUser)]
        public ActionResult SubmitCreateUser(CreateUserViewModel createUserViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View("CreateUser");
                }


                bool success = mainUserService.CreateUser(createUserViewModel);

                if (success)
                {
                    return View("Index");
                }
                else
                {
                    TempData["Message"] = "帳號重複，創建失敗";
                    return RedirectToAction("CreateUser");
                }

            }
            catch (Exception e)
            {
                ViewBag.Message = "創建失敗";
                logger.Error(e);
                return View("CreateUser");
                throw e;
            }
        }

        /// <summary>
        /// 提交修改角色權限
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="Role"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        [UserRoleAuthFilter(UserPermission.EditUser)]
        public ActionResult SubmitEditUserRole(string UserId, string RoleId, string Status)
        {
            try
            {

                mainUserService.EditUserRole(UserId, RoleId, Status);
                return View("Index");
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
        /// <param name="UserId"></param>
        /// <returns></returns>
        [UserRoleAuthFilter(UserPermission.DelUser)]
        public ActionResult DeleteUser(string UserId)
        {
            try
            {
                mainUserService.DeleteUser(UserId);
                return View("Index");
            }
            catch (Exception e)
            {
                logger.Error(e);
                return View("Index");
                throw e;
            }
        }

        /// <summary>
        /// 根據UserId查詢使用者
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public ActionResult SelectUser(string UserId)
        {
            try
            {
                User user = mainUserService.GetUser(UserId);

                if (user == null)
                {
                    //沒找到對應使用者
                    string noUser = "noUser";
                    return Json(noUser, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(user, JsonRequestBehavior.AllowGet);

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
        /// 到改密碼頁面
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public ActionResult EditUserPwd()
        {
            return View();
        }

        /// <summary>
        /// 提交修改密碼
        /// </summary>
        /// <param name="OldPwd"></param>
        /// <param name="NewPwd"></param>
        /// <returns></returns>
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