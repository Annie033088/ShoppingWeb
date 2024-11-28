using NLog;
using Pashamao.Models;
using Pashamao.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pashamao.Service
{
    public class MainUserService
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger ();
        private UserRepository userRepository;
        internal MainUserService() { 
            userRepository = new UserRepository();
        }

        /// <summary>
        /// 取得所有用戶
        /// </summary>
        /// <returns></returns>
        internal List<User> GetAllUsers() {
            return userRepository.GetAll().ToList();
        }

        /// <summary>
        /// 新增用戶
        /// </summary>
        /// <param name="createUserViewModel"></param>
        internal bool CreateUser( CreateUserViewModel createUserViewModel ) {
            User user = new User();
            try
            {
                UserRole roleId = (UserRole)Enum.Parse ( typeof ( UserRole ), createUserViewModel.DropDownRole );
                user.Account = createUserViewModel.CreateAcct;
                user.Pwd = createUserViewModel.CreatePwd;
                user.Name = createUserViewModel.CreateName == null ? string.Empty : createUserViewModel.CreateName;
                user.RoleId = (int)roleId;

                logger.Trace("CreateUser");
                return userRepository.Create ( user );
            } catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 刪除用戶
        /// </summary>
        /// <param name="UID"></param>
        internal void DeleteUser(string UID)
        {
            User user = new User();
            user.UID = int.Parse ( UID );
            userRepository.Delete ( user );
        }
        
        /// <summary>
        /// 修改用戶名稱跟角色
        /// </summary>
        /// <param name="UID"></param>
        /// <param name="Name"></param>
        /// <param name="Status"></param>
        internal void EditUserRole( string UID,string Role, string Status)
        {
            User user = new User();
            UserRole roleId = (UserRole)Enum.Parse ( typeof ( UserRole ), Role );

            user.UID = int.Parse ( UID );
            user.RoleId =(int)roleId;
            user.Status = bool.Parse ( Status );
            userRepository.UpdateRole ( user );
        }

        /// <summary>
        /// 取得指定用戶資料
        /// </summary>
        /// <param name="UID"></param>
        /// <returns></returns>
        internal User GetUser( string UID ) { 
            int Uid = int.Parse ( UID );
            return userRepository.Get ( Uid );
        }
    }
}