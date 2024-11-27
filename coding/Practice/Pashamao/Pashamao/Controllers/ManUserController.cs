using Pashamao.Filters;
using Pashamao.Models;
using Pashamao.Service;
using System;
using System.Web.Mvc;

namespace Pashamao.Controllers
{
    [UserKickOutFilter]
    public class ManUserController : Controller
    {
        private ManUserService manUserService;
        public ManUserController()
        {
            manUserService = new ManUserService();
        }

        /// <summary>
        /// 後端使用者管理主頁
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 取得所有使用者資料
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAllUser()
        {
            return Json(manUserService.GetAllUsers(), JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 創造新的使用者
        /// </summary>
        /// <returns></returns>
        [UserRoleAuthFilter(UserRole.SuperAdmin)]
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

            try
            {
                bool success = manUserService.CreateUser(createUserViewModel);

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
        /// 修改使用者
        /// </summary>
        /// <param name="UID"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        [UserRoleAuthFilter(UserRole.SuperAdmin)]
        public ActionResult EditUser(string UID, string Status)
        {
            ViewBag.UID = UID;
            ViewBag.Status = Status;
            return View();
        }

        /// <summary>
        /// 提交修改使用者表單
        /// </summary>
        /// <param name="UID"></param>
        /// <param name="Role"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        public ActionResult SubmitEditUser(string UID, string Role, string Status)
        {
            manUserService.EditUser(UID, Role, Status);
            return View("Index");
        }

        /// <summary>
        /// 刪除使用者
        /// </summary>
        /// <param name="UID"></param>
        /// <returns></returns>
        [UserRoleAuthFilter(UserRole.SuperAdmin)]
        public ActionResult DeleteUser(string UID)
        {
            manUserService.DeleteUser(UID);
            return View("Index");
        }

        /// <summary>
        /// 根據Uid查詢使用者
        /// </summary>
        /// <param name="UID"></param>
        /// <returns></returns>
        public ActionResult SelectUser(string UID)
        {

            if (manUserService.GetUser(UID) == null)
            {
                //沒找到對應使用者
                string noUser = "noUser";
                return Json(noUser, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(manUserService.GetUser(UID), JsonRequestBehavior.AllowGet);

            }

        }

    }
}