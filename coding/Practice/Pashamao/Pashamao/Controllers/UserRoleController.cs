﻿using Pashamao.Filters;
using Pashamao.Models;
using Pashamao.Service;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Pashamao.Controllers
{
    [UserKickOutFilter]
    public class UserRoleController : Controller
    {
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
            return Json(userRoleService.GetAllRole(), JsonRequestBehavior.AllowGet);
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
            userRoleService.AddRole(selectedCkbs, roleName, roleDiscript);
            return View("Index");
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
            catch (System.Exception e)
            {

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
            userRoleService.EditRole(selectedCkbs, roleId, roleName, roleDiscript);
            return View("Index");
        }

        /// <summary>
        /// 刪除角色
        /// </summary>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteRole(string RoleId) {

            return View("Index");
        }

    }
}