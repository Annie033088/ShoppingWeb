using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pashamao.Models.Dto.OrderDto
{
    public class ResponseMainOrderDto
    {
        public ResponseMainOrderDto(Order order)
        {
            OrderId = order.OrderId;
            MemberId = order.MemberId;
            OrderNumber = order.OrderNumber.ToString();
            TotalPrice = order.TotalPrice;
            State = order.State;
            RecipientName = order.RecipientName;
            Phone = order.Phone;
            CreateTime = order.CreateTime;
        }

        /// <summary>
        /// 訂單Id
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// 會員Id
        /// </summary>
        public int MemberId { get; set; }

        /// <summary>
        /// 訂單編號
        /// </summary>
        public string OrderNumber { get; set; }

        /// <summary>
        /// 總金額
        /// </summary>
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// 訂單狀態
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// 收件人
        /// </summary>
        public string RecipientName { get; set; }

        /// <summary>
        /// 收件人電話
        /// </summary>
        public int Phone { get; set; }

        /// <summary>
        /// 創建時間
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}