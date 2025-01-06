using NLog;
using Pashamao.Models;
using Pashamao.Models.Dto.UserDto;
using Pashamao.Repositories;
using System;
using System.Collections.Generic;
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
        /// 取得搜尋之前的使用者資料
        /// </summary>
        internal (List<User> users, int totalPage) GetSortedUser(RequestGetSortedUserDto sortedUserDto)
        {
            try
            {
                bool haveThisColumn = false;

                if (sortedUserDto.SortColumn == "UserId")
                {
                    sortedUserDto.SortColumn = "f_userId";
                    haveThisColumn = true;
                }

                if (sortedUserDto.SortColumn == "Account")
                {
                    sortedUserDto.SortColumn = "f_account";
                    haveThisColumn = true;
                }

                if (sortedUserDto.SortColumn == "Name")
                {
                    sortedUserDto.SortColumn = "f_name";
                    haveThisColumn = true;
                }

                if (sortedUserDto.SortColumn == "Status")
                {
                    sortedUserDto.SortColumn = "f_status";
                    haveThisColumn = true;
                }

                if (sortedUserDto.SortColumn == "RoleId")
                {
                    sortedUserDto.SortColumn = "f_roleId";
                    haveThisColumn = true;
                }

                if (haveThisColumn)
                {
                    return userRepository.GetSortedUser(sortedUserDto);
                }

                return (null, 0);
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }
        }

        /// <summary>
        /// 取得指定用戶資料(並排序)
        /// </summary>
        internal (List<User> users, int totalPage) SelectUser(RequestSelectUserDto selectUserDto)
        {
            try
            {
                bool haveThisColumn = false;

                //以下是目前有的搜尋欄位, 如果要擴充, 需要注意先把欄位level跟status進行轉換判斷, 可以轉換成byte(tinyint)或者bool(bit)
                if (selectUserDto.SelectColumn == "UserId")
                {
                    selectUserDto.SelectColumn = "f_userId";
                    haveThisColumn = true;
                }

                //根據甚麼欄位進行排序
                if (selectUserDto.SortColumn == "UserId")
                {
                    selectUserDto.SortColumn = "f_userId";
                    haveThisColumn = true;
                }
                else if (selectUserDto.SortColumn == "Account")
                {
                    selectUserDto.SortColumn = "f_account"; haveThisColumn = true;
                }
                else if (selectUserDto.SortColumn == "Name")
                {
                    selectUserDto.SortColumn = "f_name";
                    haveThisColumn = true;
                }
                else if (selectUserDto.SortColumn == "Status")
                {
                    selectUserDto.SortColumn = "f_status";
                    haveThisColumn = true;
                }
                else if (selectUserDto.SortColumn == "RoleId")
                {
                    selectUserDto.SortColumn = "f_roleId";
                    haveThisColumn = true;
                }

                if (haveThisColumn)
                {
                    return userRepository.GetSelectUser(selectUserDto);
                }

                return (null, 0);
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
        internal bool CreateUser(RequestCreateUserDto createUserDto)
        {
            try
            {
                User user = new User
                {
                    Account = createUserDto.Account,
                    Pwd = createUserDto.Pwd,
                    Name = createUserDto.Name == null ? string.Empty : createUserDto.Name,
                    RoleId = int.Parse(createUserDto.DropDownRole)
                };
                logger.Trace("CreateUser");
                return userRepository.CreateUser(user);
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
        internal bool DeleteUser(int userId)
        {
            try
            {
                logger.Trace("DeleteUser");
                return userRepository.DeleteUser(userId);
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
        internal List<Role> GetRoleIdAndName()
        {
            try
            {
                return userRepository.GetRoleIdAndName();
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
        internal bool EditUserRoleAndStatus(RequestEditUserRoleAndStatusDto editUserRoleAndStatus)
        {
            try
            {
                return userRepository.EditUserRoleAndStatus(editUserRoleAndStatus);
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
        internal bool EditUserPwd(RequestEditUserPwdDto editUserPwdDto)
        {
            try
            {
                UserSessionModel userModel = HttpContext.Current.Session["UserSession"] as UserSessionModel;
                return userRepository.UpdatePwd(userModel.UserId, editUserPwdDto);
            }
            catch (Exception e)
            {
                logger.Error(e);
                throw e;
            }
        }
    }
}