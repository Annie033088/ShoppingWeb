using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Pashamao.Models.Dto.RoleDto
{
    public class RequestEditRoleDto
    {
        /// <summary>
        /// 修改的角色Id
        /// </summary>
        [Required]
        public int RoleId { get; set; }

        /// <summary>
        /// 修改的權限
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