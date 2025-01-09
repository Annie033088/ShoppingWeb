using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        [RegularExpression("^.{1,30}$", ErrorMessage = "長度上限為30")]
        public string Name { get; set; }

        /// <summary>
        /// 角色描述
        /// </summary>
        [Required]
        [RegularExpression("^.{1,30}$", ErrorMessage = "長度上限為60")]
        public string Description { get; set; }

    }
}