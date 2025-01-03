using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Pashamao.Models.Dto.MemberDto
{
    public class RequestEditMemberLevelAndStatusDto
    {
        /// <summary>
        /// 會員Id
        /// </summary>
        [Required]
        public int MemberId { get; set; }

        /// <summary>
        /// 狀態
        /// </summary>
        [Required]
        public bool Status { get; set; }

        /// <summary>
        /// 等級
        /// </summary>
        [Required]
        public int Level { get; set; }
    }
}