using System.ComponentModel.DataAnnotations;

namespace Pashamao.Models.Dto.RoleDto
{
    public class RequestGeneralRoleIdDto
    {
        /// <summary>
        /// 角色Id
        /// </summary>
        [Required]
        public int RoleId { get; set; }
    }
}