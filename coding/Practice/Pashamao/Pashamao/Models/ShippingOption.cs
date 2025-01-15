using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pashamao.Models
{
    public class ShippingOption
    {
        /// <summary>
        /// 運輸方式Id
        /// </summary>
        public int ShippingOptionId {  get; set; }

        /// <summary>
        /// 運輸方式
        /// </summary>
        public string Option { get; set; }

        /// <summary>
        /// 運費
        /// </summary>
        public decimal ShippingFee {  get; set; }

        /// <summary>
        /// 免運費金額
        /// </summary>
        public decimal FreeShipping { get; set; }

        /// <summary>
        /// 創建時間
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 最後修改時間
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}