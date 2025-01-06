using NLog;
using Pashamao.Filters;
using Pashamao.Models;
using Pashamao.Models.Dto.RoleDto;
using Pashamao.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

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
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 取得所有角色資料
        /// </summary>
        [HttpPost]
        public ActionResult GetAllRole()
        {
            try
            {
                List<Role> roles = userRoleService.GetAllRole();
                List<ResponseMainRoleDto> mainRoleDtos = roles.Select(role => (new ResponseMainRoleDto(role))).ToList();
                return Json(new { roles = mainRoleDtos });
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
        /// 新增用戶
        /// </summary>
        [HttpPost]
        public ActionResult AddRole(RequestAddRoleDto addRoleDto)
        {
            try
            {
                //檢查前端資料
                if (!ModelState.IsValid)
                {
                    string errorMessage = "無效的輸入格式";
                    return Json(new { errorMessage });
                }

                bool successFlag = userRoleService.AddRole(addRoleDto);

                if (successFlag)
                {
                    return Json(new { successFlag });
                }
                else
                {
                    string errorMessage = "新增失敗";
                    return Json(new { errorMessage });
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
        /// 取得選擇角色的權限(內容)
        /// </summary>
        [HttpPost]
        public ActionResult GetRolePermissions(int roleId)
        {
            try
            {
                if (roleId < 0)
                {
                    string errorMessage = "無效的輸入";
                    return Json(new { errorMessage });
                }

                return Json(new { rolePermissions = userRoleService.GetRolePermissions(roleId) });
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
        /// 修改角色權限
        /// </summary>
        [HttpPost]
        public ActionResult EditRole(RequestEditRoleDto editRoleDto)
        {
            try
            {
                //檢查前端資料
                if (!ModelState.IsValid)
                {
                    string errorMessage = "無效的輸入";
                    return Json(new { errorMessage });
                }

                bool successFlag = userRoleService.EditRole(editRoleDto);

                if (successFlag)
                {
                    return Json(new { successFlag });
                }
                else
                {
                    string errorMessage = "修改失敗";
                    return Json(new { errorMessage });
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
        /// 刪除角色
        /// </summary>
        [HttpPost]
        public ActionResult DeleteRole(int roleId)
        {
            try
            {
                if (roleId < 0)
                {
                    string errorMessage = "無效的輸入";
                    return Json(new { errorMessage });
                }

                bool successFlag = userRoleService.DeleteRole(roleId);

                if (successFlag)
                {
                    return Json(new { successFlag });
                }
                else
                {
                    string errorMessage = "刪除失敗，請再試一次";
                    return Json(new { errorMessage });
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
        /// 搜尋角色
        /// </summary>
        [HttpPost]
        public ActionResult SelectRole(int roleId)
        {
            try
            {
                if (roleId < 0)
                {
                    string errorMessage = "無效的輸入";
                    return Json(new { errorMessage });
                }

                Role role = userRoleService.GetRoleById(roleId);

                if (role == null)
                {
                    string errorMessage = "沒有這個角色";
                    return Json(new { errorMessage });
                }
                else
                {
                    ResponseMainRoleDto mainRoleDtos = new ResponseMainRoleDto(role);
                    return Json(new { role = mainRoleDtos });
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
    }
}