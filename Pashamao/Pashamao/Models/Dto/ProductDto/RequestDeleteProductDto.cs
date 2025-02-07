using System;
using System.ComponentModel.DataAnnotations;

namespace Pashamao.Models.Dto.ProductDto
{
    public class RequestDeleteProductDto
    {
        /// <summary>
        /// 商品Id
        /// </summary>
        [Required]
        public Guid ProductId { get; set; }
    }
}