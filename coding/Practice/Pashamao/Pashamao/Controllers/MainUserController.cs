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
                //檢查前端資料
                if (!ModelState.IsValid)
                {
                    string errorMessage = "無效的輸入格式";
                    return Json(new { errorMessage });
                }

                (List<User> users, int totalPage) = mainUserService.GetSortedUser(sortedUserDto);
                List<ResponseMainUserDto> mainUserDto = users.Select(user => (new ResponseMainUserDto(user))).ToList();
                return Json((new { users = mainUserDto, totalPage }));
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
        /// 根據欄位查詢排序後使用者
        /// </summary>
        [HttpPost]
        public ActionResult SelectUser(RequestSelectUserDto selectUserDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    string errorMessage = "無效的輸入格式";
                    return Json(new { errorMessage });
                }

                (List<User> users, int totalPage) = mainUserService.SelectUser(selectUserDto);

                if (users == null)
                {
                    string errorMessage = "沒找到使用者";
                    return Json(new { errorMessage });
                }
                else
                {
                    List<ResponseMainUserDto> mainUserDto = users.Select(user => (new ResponseMainUserDto(user))).ToList();
                    return Json((new { users = mainUserDto, totalPage }));
                }
            }
            catch (Exception e)
            {
                string errorMessage = "發生錯誤，請再試一次";
                logger.Error(e);
                return Json(errorMessage);
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
                    TempData["Message"] = "無效的輸入格式";
                    return RedirectToAction("GetCreateUserView");
                }

                //檢查roleId是否符合規範
                int roleId = 0;
                if (!int.TryParse(createUserDto.DropDownRole, out roleId))
                {
                    TempData["Message"] = "無效的輸入格式";
                    return RedirectToAction("GetCreateUserView");
                }
                
                bool successFlag = mainUserService.CreateUser(createUserDto);

                if (successFlag)
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
                TempData["Message"] = "發生錯誤，請再試一次";
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
                    string errorMessage = "無效的輸入格式";
                    return Json(new { errorMessage });
                }

                bool successFlag = mainUserService.EditUserRoleAndStatus(editUserRoleAndStatus);

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
                string errorMessage = "發生錯誤, 請再試一次";
                logger.Error(e);
                return Json(new { errorMessage });
                throw e;
            }
        }

        /// <summary>
        /// 刪除使用者
        /// </summary>
        [UserRoleAuthFilter(UserPermission.DelUser)]
        public ActionResult DeleteUser(RequestDeleteUserDto userId)
        {
            try
            {
                if (!ModelState.IsValid || userId.UserId < 0)
                {
                    string errorMessage = "無效的輸入格式";
                    return Json(new { errorMessage });
                }

                bool successFlag = mainUserService.DeleteUser(userId.UserId);

                if (successFlag)
                {
                    return Json(new { successFlag });
                }
                else
                {
                    string errorMessage = "刪除失敗";
                    return Json(new { errorMessage });
                }
            }
            catch (Exception e)
            {
                string errorMessage = "發生錯誤，請再試一次";
                logger.Error(e);
                return Json(errorMessage);
                throw e;
            }
        }

        /// <summary>
        /// 到改密碼頁面
        /// </summary>
        public ActionResult GetEditUserPwdView()
        {
            return View("EditUserPwd");
        }

        /// <summary>
        /// 提交修改密碼
        /// </summary>
        [HttpPost]
        public ActionResult SubmitEditUserPwd(RequestEditUserPwdDto editUserPwdDto)
        {
            try
            {
                if (editUserPwdDto.OldPwd == editUserPwdDto.NewPwd)
                {
                    ViewBag.Message = "密碼輸入重複";
                    return View("EditUserPwd");
                }

                if (mainUserService.EditUserPwd(editUserPwdDto))
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