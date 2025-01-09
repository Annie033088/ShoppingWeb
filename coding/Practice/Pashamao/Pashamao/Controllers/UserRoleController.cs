using NLog;
using Pashamao.Filters;
using Pashamao.Models;
using Pashamao.Models.Dto.RoleDto;
using Pashamao.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

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
        public ActionResult GetAllRole(RequestGetAllRoleDto getAllRoleDto)
        {
            try
            {
                ErrorCodeDefine errorCode = 0;
                //檢查前端資料
                if (!ModelState.IsValid)
                {
                    errorCode = ErrorCodeDefine.InvalidFormatOrEntry;
                    return Json(new { errorCode });
                }

                (List<Role> roles, int totalPage) = userRoleService.GetAllRole(getAllRoleDto);
                List<ResponseMainRoleDto> mainRoleDtos = roles.Select(role => (new ResponseMainRoleDto(role))).ToList();

                errorCode = ErrorCodeDefine.Success;
                return Json(new { roles = mainRoleDtos, totalPage, errorCode });
            }
            catch (Exception e)
            {
                ErrorCodeDefine errorCode = ErrorCodeDefine.ServerError;
                logger.Error(e);
                return Json(new { errorCode });
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
                ErrorCodeDefine errorCode = 0;
                //檢查前端資料
                if (!ModelState.IsValid)
                {
                    errorCode = ErrorCodeDefine.InvalidFormatOrEntry;
                    return Json(new { errorCode });
                }

                bool successFlag = userRoleService.AddRole(addRoleDto);

                if (successFlag)
                {
                    errorCode = ErrorCodeDefine.Success;
                    return Json(new { errorCode });
                }
                else
                {
                    errorCode = ErrorCodeDefine.CreateFailed;
                    return Json(new { errorCode });
                }
            }
            catch (Exception e)
            {
                ErrorCodeDefine errorCode = ErrorCodeDefine.ServerError;
                logger.Error(e);
                return Json(new { errorCode });
                throw e;
            }
        }

        /// <summary>
        /// 取得選擇角色的權限(內容)
        /// </summary>
        [HttpPost]
        public ActionResult GetRolePermissions(RequestGeneralRoleIdDto roleIdDto)
        {
            try
            {
                ErrorCodeDefine errorCode = 0;
                //檢查前端資料
                if (!ModelState.IsValid || roleIdDto.RoleId < 0)
                {
                    errorCode = ErrorCodeDefine.InvalidFormatOrEntry;
                    return Json(new { errorCode });
                }

                errorCode = ErrorCodeDefine.Success;
                return Json(new { rolePermissions = userRoleService.GetRolePermissions(roleIdDto.RoleId), errorCode });
            }
            catch (Exception e)
            {
                ErrorCodeDefine errorCode = ErrorCodeDefine.ServerError;
                logger.Error(e);
                return Json(new { errorCode });
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
                ErrorCodeDefine errorCode = 0;
                //檢查前端資料
                if (!ModelState.IsValid || editRoleDto.RoleId < 0)
                {
                    errorCode = ErrorCodeDefine.InvalidFormatOrEntry;
                    return Json(new { errorCode });
                }

                bool successFlag = userRoleService.EditRole(editRoleDto);

                if (successFlag)
                {
                    errorCode = ErrorCodeDefine.Success;
                    return Json(new { errorCode });
                }
                else
                {
                    errorCode = ErrorCodeDefine.ModifiedFailed;
                    return Json(new { errorCode });
                }
            }
            catch (Exception e)
            {
                ErrorCodeDefine errorCode = ErrorCodeDefine.ServerError;
                logger.Error(e);
                return Json(new { errorCode });
                throw e;
            }
        }

        /// <summary>
        /// 刪除角色
        /// </summary>
        [HttpPost]
        public ActionResult DeleteRole(RequestGeneralRoleIdDto roleIdDto)
        {
            try
            {
                ErrorCodeDefine errorCode = 0;
                //檢查前端資料
                if (!ModelState.IsValid || roleIdDto.RoleId < 0)
                {
                    errorCode = ErrorCodeDefine.InvalidFormatOrEntry;
                    return Json(new { errorCode });
                }

                bool successFlag = userRoleService.DeleteRole(roleIdDto.RoleId);

                if (successFlag)
                {
                    errorCode = ErrorCodeDefine.Success;
                    return Json(new { errorCode });
                }
                else
                {
                    errorCode = ErrorCodeDefine.DeleteFailed;
                    return Json(new { errorCode });
                }

            }
            catch (Exception e)
            {
                ErrorCodeDefine errorCode = ErrorCodeDefine.ServerError;
                logger.Error(e);
                return Json(new { errorCode });
                throw e;
            }
        }

        /// <summary>
        /// 搜尋角色
        /// </summary>
        [HttpPost]
        public ActionResult SelectRole(RequestGeneralRoleIdDto roleIdDto)
        {
            try
            {
                ErrorCodeDefine errorCode = 0;
                //檢查前端資料
                if (!ModelState.IsValid || roleIdDto.RoleId < 0)
                {
                    errorCode = ErrorCodeDefine.InvalidFormatOrEntry;
                    return Json(new { errorCode });
                }

                Role role = userRoleService.GetRoleById(roleIdDto.RoleId);

                if (role == null)
                {
                    errorCode = ErrorCodeDefine.Success;
                    return Json(new { errorCode });
                }
                else
                {
                    ResponseMainRoleDto mainRoleDtos = new ResponseMainRoleDto(role);
                    errorCode = ErrorCodeDefine.Success;
                    return Json(new { role = mainRoleDtos, errorCode });
                }
            }
            catch (Exception e)
            {
                ErrorCodeDefine errorCode = ErrorCodeDefine.ServerError;
                logger.Error(e);
                return Json(new { errorCode });
                throw e;
            }
        }
    }
}