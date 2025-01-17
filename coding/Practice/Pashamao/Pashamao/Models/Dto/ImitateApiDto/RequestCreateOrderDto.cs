using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Pashamao.Models.Dto.ImitateApiDto
{
    public class RequestCreateOrderDto
    {
        /// <summary>
        /// 會員Id
        /// </summary>
        [Required]
        public int MemberId { get; set; }

        /// <summary>
        /// 收件人
        /// </summary>
        [RegularExpression("^.{1,50}$", ErrorMessage = "至多50字")]
        public string Name { get; set; }

        /// <summary>
        /// 郵遞區號
        /// </summary>
        [Required]
        [RegularExpression("^[0-9]{3,6}$", ErrorMessage = "郵遞區號錯誤")]
        public int PostalCode { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [RegularExpression("^.{1,326}$", ErrorMessage = "地址錯誤")]
        public string Address { get; set; }

        /// <summary>
        /// 電話
        /// </summary>
        [Required]
        [RegularExpression("^[0-9]{9}$", ErrorMessage = "電話錯誤")]
        public int Phone { get; set; }

        /// <summary>
        /// 運送方式
        /// </summary>
        [Required]
        public int ShippingOptionId { get; set; }

        /// <summary>
        /// 子項目(細項)
        /// </summary>
        public List<RequestCreateOrderProductDto> createOrderProductDtos { get; set; }
    }

    public class RequestCreateOrderProductDto
    {
        /// <summary>
        /// 細項Id
        /// </summary>
        public int ProductStyleId { get; set; }

        /// <summary>
        /// 商品Id
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// 庫存
        /// </summary>
        public int Quantity { get; set; }
    }
}