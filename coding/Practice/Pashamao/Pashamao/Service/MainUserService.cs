using NLog;
using Pashamao.Models;
using Pashamao.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

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
        /// 取得所有用戶
        /// </summary>
        /// <returns></returns>
        internal List<User> GetAllUsers()
        {
            try
            {
                return userRepository.GetAll().ToList();
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
        /// 取得指定用戶資料
        /// </summary>
        /// <param name="UID"></param>
        /// <returns></returns>
        internal User GetUser(string UserId)
        {
            try
            {
                int Uid = int.Parse(UserId);
                return userRepository.Get(Uid);
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