using Newtonsoft.Json;
using NLog;
using Pashamao.Filters;
using Pashamao.Models;
using Pashamao.Models.Dto.UserDto;
using Pashamao.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace Pashamao.Controllers
{
    [UserKickOutFilter]
    [UserRoleAuthFilter(UserPermission.CreateUser | UserPermission.DelUser | UserPermission.EditUser | UserPermission.SelectUser)]
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
                int errorCode = 0;
                //檢查前端資料
                if (!ModelState.IsValid)
                {
                    errorCode = 5;
                    return Json(new { errorCode });
                }

                (List<User> users, int totalPage) = mainUserService.GetSortedUser(sortedUserDto);
                List<ResponseMainUserDto> mainUserDto = users.Select(user => (new ResponseMainUserDto(user))).ToList();

                errorCode = 1;
                return Json((new { users = mainUserDto, totalPage, errorCode }));
            }
            catch (Exception e)
            {
                int errorCode = 6;
                logger.Error(e);
                return Json(new { errorCode });
                throw e;
            }
        }

        /// <summary>
        /// 根據欄位查詢排序後使用者
        /// </summary>
        [HttpPost]
        public ActionResult SelectUser(RequestSelectUserDto selectUserDto)
        {
            try
            {
                int errorCode = 0;
                //檢查前端資料
                if (!ModelState.IsValid)
                {
                    errorCode = 5;
                    return Json(new { errorCode });
                }

                (List<User> users, int totalPage) = mainUserService.SelectUser(selectUserDto);

                if (users == null)
                {
                    errorCode = 1;
                    return Json(new { errorCode });
                }
                else
                {
                    List<ResponseMainUserDto> mainUserDto = users.Select(user => (new ResponseMainUserDto(user))).ToList();
                    errorCode = 1;
                    return Json((new { users = mainUserDto, totalPage, errorCode }));
                }
            }
            catch (Exception e)
            {
                int errorCode = 6;
                logger.Error(e);
                return Json(new { errorCode });
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

                ViewBag.JsonRoles = JsonConvert.SerializeObject(new { roles = roleIdAndNameDto });
                return View("CreateUser");
            }
            catch (Exception e)
            {
                logger.Error(e);
                int errorCode = 6;
                return Json(new { errorCode });
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
                int errorCode = 0;

                if (!ModelState.IsValid || createUserDto.RoleId < 0)
                {
                    errorCode = 5;
                    return Json(new { errorCode });
                }
                
                bool successFlag = mainUserService.CreateUser(createUserDto);

                if (successFlag)
                {
                    errorCode = 1;
                    return Json(new { errorCode });
                }
                else
                {
                    errorCode = 11;
                    return Json(new { errorCode });
                }
            }
            catch (Exception e)
            {
                logger.Error(e);
                int errorCode = 6;
                return Json(new { errorCode });
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
                int errorCode = 0;

                if (!ModelState.IsValid)
                {
                    errorCode = 5;
                    return Json(new { errorCode });
                }

                bool successFlag = mainUserService.EditUserRoleAndStatus(editUserRoleAndStatus);

                if (successFlag)
                {
                    errorCode = 1;
                    return Json(new { errorCode });
                }
                else
                {
                    errorCode = 12;
                    return Json(new { errorCode });
                }
            }
            catch (Exception e)
            {
                int errorCode = 6;
                logger.Error(e);
                return Json(new { errorCode });
                throw e;
            }
        }

        /// <summary>
        /// 刪除使用者
        /// </summary>
        [UserRoleAuthFilter(UserPermission.DelUser)]
        public ActionResult DeleteUser(RequestDeleteUserDto userIdDto)
        {
            try
            {
                int errorCode = 0;

                if (!ModelState.IsValid || userIdDto.UserId < 0)
                {
                    errorCode = 5;
                    return Json(new { errorCode });
                }

                bool successFlag = mainUserService.DeleteUser(userIdDto.UserId);

                if (successFlag)
                {
                    errorCode = 1;
                    return Json(new { errorCode });
                }
                else
                {
                    errorCode = 13;
                    return Json(new { errorCode });
                }
            }
            catch (Exception e)
            {
                int errorCode = 6;
                logger.Error(e);
                return Json(new { errorCode });
                throw e;
            }
        }
    }
}