using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pashamao.Models.Dto.UserDto
{
    public class ResponseRoleIdAndNameDto
    {
        public ResponseRoleIdAndNameDto(Role role)
        {
            RoleId = role.RoleId;
            Name = role.Name;
        }

        /// <summary>
        /// roleId
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// 名
        /// </summary>
        public string Name { get; set; }
    }
}