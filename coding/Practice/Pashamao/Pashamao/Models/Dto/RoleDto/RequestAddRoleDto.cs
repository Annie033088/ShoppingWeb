using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Pashamao.Models.Dto.RoleDto
{
    public class RequestAddRoleDto
    {
        /// <summary>
        /// 新增的權限
        /// </summary>
        [Required]
        public List<string> PermissionCkbs { get; set; }

        /// <summary>
        /// 角色名稱
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 角色描述
        /// </summary>
        [Required]
        public string Description { get; set; }

    }
}