using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

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