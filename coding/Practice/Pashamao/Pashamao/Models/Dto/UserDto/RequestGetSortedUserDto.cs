using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Pashamao.Models.Dto.UserDto
{
    public class RequestGetSortedUserDto
    {
        /// <summary>
        /// 排序的欄位
        /// </summary>
        [Required]
        public string SortColumn { get; set; }

        /// <summary>
        /// 第幾頁
        /// </summary>
        [Required]
        public int Page { get; set; }

        /// <summary>
        /// 升序或降序
        /// </summary>
        [Required]
        public string SortOrder { get; set; }
    }
}