using System;

namespace Pashamao.Models.Dto.OrderDto
{
    public class ResponseMainOrderDto
    {
        public ResponseMainOrderDto(Order order)
        {
            OrderId = order.OrderId;
            MemberId = order.MemberId;
            OrderNumber = order.OrderNumber.ToString();
            TotalAmount = order.TotalAmount;
            PreviousState = order.PreviousState;
            CurrentState = order.CurrentState;
            RecipientName = order.RecipientName;
            Phone = order.Phone;
            CreateTime = order.CreateTime;
            UpdateTime = order.UpdateTime;
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
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// 前一個訂單狀態
        /// </summary>
        public int PreviousState { get; set; }

        /// <summary>
        /// 目前訂單狀態
        /// </summary>
        public int CurrentState { get; set; }

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

        /// <summary>
        /// 最後修改時間
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}