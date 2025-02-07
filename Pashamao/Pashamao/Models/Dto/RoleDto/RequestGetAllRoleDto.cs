using System.ComponentModel.DataAnnotations;

namespace Pashamao.Models.Dto.RoleDto
{
    public class RequestGetAllRoleDto
    {
        /// <summary>
        /// 第幾頁
        /// </summary>
        [Required]
        public int Page { get; set; }
    }
}