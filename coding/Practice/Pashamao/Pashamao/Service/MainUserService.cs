using Newtonsoft.Json.Linq;
using NLog;
using Pashamao.Models;
using Pashamao.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace Pashamao.Service
{
    public class MainUserService
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger();
        private UserRepository userRepository;
        internal MainUserService()
        {
            userRepository = new UserRepository();
        }



        /// <summary>
        /// 取得搜尋之前的使用者資料
        /// </summary>
        /// <param name="column"></param>
        /// <param name="page"></param>
        /// <param name="sortOrder"></param>
        /// <returns></returns>
        internal (List<User>, int) GetSortedUser(string column, string page, string sortOrder)
        {
            if (column == "UserId") column = "f_uid";
            if (column == "Account") column = "f_account";
            if (column == "Name") column = "f_name";
            if (column == "Status") column = "f_status";
            if (column == "RoleId") column = "f_roleId";

            return userRepository.GetSortedUser(column, int.Parse(page), sortOrder);
        }

        /// <summary>
        /// 取得指定用戶資料(並排序)
        /// </summary>
        /// <param name="UID"></param>
        /// <returns></returns>
        internal (List<User>, int) SelectUser(string selectColumn, string value, string sortColumn, string page, string sortOrder)
        {
            try
            {
                //以下是目前有的搜尋欄位, 如果要擴充, 需要注意先把欄位level跟status進行轉換判斷, 可以轉換成byte(tinyint)或者bool(bit)
                if (selectColumn == "UserId")
                {
                    selectColumn = "f_uid";
                }

                //根據甚麼欄位進行排序
                if (sortColumn == "UserId") sortColumn = "f_uid";
                if (sortColumn == "Account") sortColumn = "f_account";
                if (sortColumn == "Name") sortColumn = "f_name";
                if (sortColumn == "Status") sortColumn = "f_status";
                if (sortColumn == "RoleId") sortColumn = "f_roleId";
                return userRepository.GetSelectUser(selectColumn, value, sortColumn, page, sortOrder);
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        /// <summary>
        /// 新增用戶
        /// </summary>
        /// <param name="createUserViewModel"></param>
        internal bool CreateUser(CreateUserViewModel createUserViewModel)
        {
            User user = new User();
            try
            {
                user.Account = createUserViewModel.CreateAcct;
                user.Pwd = createUserViewModel.CreatePwd;
                user.Name = createUserViewModel.CreateName == null ? string.Empty : createUserViewModel.CreateName;
                user.RoleName = createUserViewModel.DropDownRole;

                logger.Trace("CreateUser");
                return userRepository.Create(user);
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }
        }

        /// <summary>
        /// 刪除用戶
        /// </summary>
        /// <param name="UID"></param>
        internal void DeleteUser(string UserId)
        {
            try
            {
                User user = new User();
                user.UserId = int.Parse(UserId);
                userRepository.Delete(user);
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }
        }

        /// <summary>
        /// 取得角色名
        /// </summary>
        /// <returns></returns>
        internal List<string> GetAllRoleName()
        {
            try
            {
                return userRepository.GetAllRoleName();
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }
        }

        /// <summary>
        /// 修改用戶角色跟狀態
        /// </summary>
        /// <param name="UID"></param>
        /// <param name="Name"></param>
        /// <param name="Status"></param>
        internal void EditUserRole(string UserId, string RoleId, string Status)
        {
            try
            {
                User user = new User();

                user.UserId = int.Parse(UserId);
                user.RoleId = int.Parse(RoleId);
                user.Status = Status == "1" ? true : false;
                userRepository.UpdateRole(user);
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }
        }


        /// <summary>
        /// 修改密碼
        /// </summary>
        /// <param name="OldPwd"></param>
        /// <param name="NewPwd"></param>
        /// <returns></returns>
        internal bool EditUserPwd(string OldPwd, string NewPwd)
        {
            try
            {
                UserSessionModel userModel = HttpContext.Current.Session["UserSession"] as UserSessionModel;
                return userRepository.UpdatePwd(userModel.UserId, OldPwd, NewPwd);
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }
        }
    }
}