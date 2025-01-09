using System.ComponentModel.DataAnnotations;

namespace Pashamao.Models.Dto.UserDto
{
    public class RequestDeleteUserDto
    {
        /// <summary>
        /// 刪除的使用者Id
        /// </summary>
        [Required]
        public int UserId { get; set; }
    }
}