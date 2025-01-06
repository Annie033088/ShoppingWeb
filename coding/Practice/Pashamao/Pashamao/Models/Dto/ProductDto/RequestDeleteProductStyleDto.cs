using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Pashamao.Models.Dto.ProductDto
{
    public class RequestDeleteProductStyleDto
    {
        /// <summary>
        /// 細項Id
        /// </summary>
        [Required]
        public int ProductStyleId {  get; set; }

        /// <summary>
        /// 商品Id
        /// </summary>
        [Required]
        public Guid ProductId { get; set; }

        /// <summary>
        /// 商品最後修改時間
        /// </summary>
        [Required]
        public DateTime LastEditTime
        { get; set; }
    }
}