using NLog;
using Pashamao.Filters;
using Pashamao.Models;
using Pashamao.Repositories;
using Pashamao.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pashamao.Service
{
    [UserAuthFilter]
    public class ManUserService
    {
        private readonly Logger logger = LogManager.GetCurrentClassLogger ();
        private UserRepository userRepository;
        internal ManUserService() { 
            userRepository = new UserRepository();
        }

        /// <summary>
        /// 取得所有用戶
        /// </summary>
        /// <returns></returns>
        internal List<Models.User> GetAllUsers() {
            return userRepository.GetAll().ToList();
        }


        /// <summary>
        /// 新增用戶
        /// </summary>
        /// <param name="createUserViewModel"></param>
        internal void CreateUser( CreateUserViewModel createUserViewModel ) {
            HashUtility hashUtility = new HashUtility();
            User user = new User();
            try
            {
                UserRole roleId = (UserRole)Enum.Parse ( typeof ( UserRole ), createUserViewModel.DropDownRole );
                user.Account = createUserViewModel.CreateAcct;
                user.Hash = hashUtility.HashPwd ( createUserViewModel.CreatePwd, hashUtility.GenerateSalt () );
                user.Name = createUserViewModel.CreateName == null ? string.Empty : createUserViewModel.CreateName;
                user.RoleId = (int)roleId;

                userRepository.Create ( user );
                logger.Trace ( "CreateUser" );
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
        internal void EditUser( string UID,string Role, string Status)
        {
            User user = new User();
            UserRole roleId = (UserRole)Enum.Parse ( typeof ( UserRole ), Role );

            user.UID = int.Parse ( UID );
            user.RoleId =(int)roleId;
            user.Status = bool.Parse ( Status );
            userRepository.Update ( user );
        }

        internal User GetUser( string UID ) { 
            int Uid = int.Parse ( UID );
            return userRepository.Get ( Uid );
        }
    }
}