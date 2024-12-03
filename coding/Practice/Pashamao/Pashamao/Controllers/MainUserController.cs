using Pashamao.Filters;
using Pashamao.Models;
using Pashamao.Service;
using System;
using System.Web.Mvc;

namespace Pashamao.Controllers
{
    [UserKickOutFilter]
    [UserRoleAuthFilter(UserPermission.CreateUser | UserPermission.DelUser | UserPermission.EditUser | UserPermission.SelectUser)]
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
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        /// <summary>
        /// 取得所有使用者資料
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAllUser()
        {
            return Json(mainUserService.GetAllUsers(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 創造新的使用者
        /// </summary>
        /// <returns></returns>
        public ActionResult CreateUser()
        {
            return View();
        }

        /// <summary>
        /// 提交創建使用者表單
        /// </summary>
        /// <param name="createUserViewModel"></param>
        /// <returns></returns>
        [HttpPost]
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
                    ViewData["Massage"] = "帳號重複，創建失敗";
                    return View("CreateUser");
                }

            }
            catch (Exception e)
            {
                ViewData["Massage"] = "創建失敗";
                return View("CreateUser");
                throw e;
            }

        }

        /// <summary>
        /// 修改角色權限
        /// </summary>
        /// <param name="UID"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        public ActionResult EditUserRole(string UID, string Status)
        {
            ViewBag.UID = UID;
            ViewBag.Status = Status;
            return View();
        }

        /// <summary>
        /// 提交修改角色權限表單
        /// </summary>
        /// <param name="UID"></param>
        /// <param name="Role"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        public ActionResult SubmitEditUserRole(string UID, string Role, string Status)
        {
            mainUserService.EditUserRole(UID, Role, Status);
            return View("Index");
        }

        /// <summary>
        /// 刪除使用者
        /// </summary>
        /// <param name="UID"></param>
        /// <returns></returns>
        public ActionResult DeleteUser(string UID)
        {
            mainUserService.DeleteUser(UID);
            return View("Index");
        }

        /// <summary>
        /// 根據Uid查詢使用者
        /// </summary>
        /// <param name="UID"></param>
        /// <returns></returns>
        public ActionResult SelectUser(string UID)
        {

            if (mainUserService.GetUser(UID) == null)
            {
                //沒找到對應使用者
                string noUser = "noUser";
                return Json(noUser, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(mainUserService.GetUser(UID), JsonRequestBehavior.AllowGet);

            }

        }

        /// <summary>
        /// 到改密碼頁面
        /// </summary>
        /// <param name="UID"></param>
        /// <returns></returns>
        public ActionResult EditUserPwd()
        {
            return View();
        }

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