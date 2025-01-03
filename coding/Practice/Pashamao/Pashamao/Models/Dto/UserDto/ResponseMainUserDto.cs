using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Pashamao.Models;

namespace Pashamao.Models.Dto.UserDto
{
    public class ResponseMainUserDto
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="user"></param>
        public ResponseMainUserDto(User user)
        {
            UserId = user.UserId;
            Account = user.Account;
            Name = user.Name;
            Status = user.Status;
            RoleId = user.RoleId;
            RoleName = user.RoleName;
        }

        /// <summary>
        /// 使用者id
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 帳號
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 狀態
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// 角色名
        /// </summary>
        public string RoleName { get; set; }
    }
}