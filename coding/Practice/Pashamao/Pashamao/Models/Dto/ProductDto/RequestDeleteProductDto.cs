using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

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