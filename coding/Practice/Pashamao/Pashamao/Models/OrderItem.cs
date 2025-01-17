using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pashamao.Models
{
    public class OrderItem
    {
        /// <summary>
        /// 訂單商品Id
        /// </summary>
        public int OrderItemId { get; set; }

        /// <summary>
        /// 訂單Id
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// 商品名稱
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 細項名
        /// </summary>
        public string Style { get; set; }

        /// <summary>
        /// 購買數量
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// 總價
        /// </summary>
        public decimal Price { get; set; }
    }
}