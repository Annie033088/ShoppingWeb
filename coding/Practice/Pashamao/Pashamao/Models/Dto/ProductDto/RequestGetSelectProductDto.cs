using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Pashamao.Models.Dto.ProductDto
{
    public class RequestGetSelectProductDto
    {
        /// <summary>
        /// 分類Id
        /// </summary>
        public int? CategoryId { get; set; }

        /// <summary>
        /// 產品Id
        /// </summary>
        public string ProductId { get; set; }

        /// <summary>
        /// 產品名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 第幾頁
        /// </summary>
        [Required]
        public int Page { get; set; }
    }
}