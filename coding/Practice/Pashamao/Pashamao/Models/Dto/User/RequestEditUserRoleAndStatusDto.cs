using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Pashamao.Models.Dto.User
{
    public class RequestEditUserRoleAndStatusDto
    {
        /// <summary>
        /// 使用者Id
        /// </summary>
        [Required]
        public int UserId {  get; set; }

        /// <summary>
        /// 腳色id
        /// </summary>
        [Required]
        public int RoleId { get; set; }

        /// <summary>
        /// 狀態
        /// </summary>
        public bool Status {  get; set; }
    }
}