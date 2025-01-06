using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Pashamao.Models.Dto.UserDto
{
    public class RequestDeleteUserDto
    {
        /// <summary>
        /// 刪除的使用者Id
        /// </summary>
        [Required]
        public int UserId {  get; set; }
    }
}