using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Pashamao.Models.Dto.OrderDto
{
    public class RequestEditShippingOptionDto
    {
        /// <summary>
        /// 運輸方式Id
        /// </summary>
        [Required]
        public int ShippingOptionId { get; set; }

        /// <summary>
        /// 運輸方式
        /// </summary>
        [RegularExpression("^.{1,10}$", ErrorMessage = "至多10字")]
        public string Option { get; set; }

        /// <summary>
        /// 運費
        /// </summary>
        [Required]
        public decimal ShippingFee { get; set; }

        /// <summary>
        /// 免運費金額
        /// </summary>
        [Required]
        public decimal FreeShipping { get; set; }

        /// <summary>
        /// 最後修改時間
        /// </summary>
        [Required]
        public DateTime UpdateTime { get; set; }
    }
}