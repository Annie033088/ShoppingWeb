using System;
using System.ComponentModel.DataAnnotations;

namespace Pashamao.Models.Dto.ProductDto
{
    public class RequestDeleteProductStyleDto
    {
        /// <summary>
        /// 細項Id
        /// </summary>
        [Required]
        public int ProductStyleId { get; set; }

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