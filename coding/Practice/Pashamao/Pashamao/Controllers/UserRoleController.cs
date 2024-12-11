using NLog;
using Pashamao.Filters;
using Pashamao.Models;
using Pashamao.Service;
using System.Collections.Generic;
using System.Web.Mvc;
using System;

namespace Pashamao.Controllers
{
    [UserKickOutFilter]
    [UserRoleAuthFilter(UserPermission.CreateUser | UserPermission.DelUser | UserPermission.EditUser)]
    public class UserRoleController : Controller
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private readonly UserRoleService userRoleService;

        public UserRoleController()
        {
            userRoleService = new UserRoleService();
        }

        /// <summary>
        /// 主頁
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        /// <summary>
        /// 取得所有角色資料
        /// </summary>
        /// <returns></returns>
        public ActionResult GetAllRole()
        {
            try
            {
                return Json(userRoleService.GetAllRole(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }
        }

        /// <summary>
        /// 新增用戶
        /// </summary>
        /// <param name="selectedCkbs"></param>
        /// <param name="roleName"></param>
        /// <param name="roleDiscript"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddRole(List<string> selectedCkbs, string roleName, string roleDiscript)
        {
            try
            {

                userRoleService.AddRole(selectedCkbs, roleName, roleDiscript);
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
        /// 取得選擇角色的權限(內容)
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetRolePermissions(string RoleId)
        {
            try
            {
                return Json(userRoleService.GetRolePermissions(RoleId), JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                logger.Error(e);
                return View("Index");
                throw e;
            }
        }

        /// <summary>
        /// 修改角色權限
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditRole(List<string> selectedCkbs, string roleId, string roleName, string roleDiscript)
        {
            try
            {
                userRoleService.EditRole(selectedCkbs, roleId, roleName, roleDiscript);
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
        /// 刪除角色
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteRole(string RoleId)
        {
            try
            {
                if (userRoleService.DeleteRole(RoleId))
                {
                    return View("Index");
                }
                else
                {
                    ViewBag.Message = "刪除失敗";
                    return View("Index");
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
        /// 搜尋角色
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        public ActionResult SelectRole(string RoleId)
        {
            try
            {
                Role role = userRoleService.GetRole(RoleId);

                if (role == null)
                {
                    string noRole = "noRole";
                    return Json(noRole, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(role, JsonRequestBehavior.AllowGet);

                }
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