using Newtonsoft.Json;
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
        private MainUserService mainUserService;
        public MainUserController()
        {
            mainUserService = new MainUserService();
        }

        /// <summary>
        /// 後端使用者管理主頁
        /// </summary>
        /// <returns></returns>
        [UserRoleAuthFilter(UserPermission.CreateUser | UserPermission.DelUser | UserPermission.EditUser | UserPermission.SelectUser)]
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 取得所有使用者資料
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetAllUser()
        {
            return Json(mainUserService.GetAllUsers(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 創造新的使用者
        /// </summary>
        /// <returns></returns>
        [UserRoleAuthFilter(UserPermission.CreateUser)]
        public ActionResult CreateUser()
        {
            ViewBag.Message = TempData["Message"];
            ViewBag.JsonRolesName = JsonConvert.SerializeObject(mainUserService.GetAllRoleName());
            return View();
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

            if (!ModelState.IsValid)
            {
                return View("CreateUser");
            }

            try
            {
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
            Console.WriteLine(UserId, RoleId, Status);

            mainUserService.EditUserRole(UserId, RoleId, Status);
            return View("Index");
        }

        /// <summary>
        /// 刪除使用者
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [UserRoleAuthFilter(UserPermission.DelUser)]
        public ActionResult DeleteUser(string UserId)
        {
            mainUserService.DeleteUser(UserId);
            return View("Index");
        }

        /// <summary>
        /// 根據UserId查詢使用者
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [UserRoleAuthFilter(UserPermission.CreateUser | UserPermission.DelUser | UserPermission.EditUser | UserPermission.SelectUser)]
        public ActionResult SelectUser(string UserId)
        {

            if (mainUserService.GetUser(UserId) == null)
            {
                //沒找到對應使用者
                string noUser = "noUser";
                return Json(noUser, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(mainUserService.GetUser(UserId), JsonRequestBehavior.AllowGet);

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

            if (mainUserService.EditUserPwd(OldPwd, NewPwd))
            {
                return View("Index");
            }

            ViewBag.Message = "密碼輸入錯誤";
            return View("EditUserPwd");
        }
    }
}