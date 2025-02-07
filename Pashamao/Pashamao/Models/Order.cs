using System;

namespace Pashamao.Models
{
    public class Order
    {
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
        public long OrderNumber { get; set; }

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
        /// 收件人地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 折扣前金額
        /// </summary>
        public decimal OriginalAmount { get; set; }

        /// <summary>
        /// 折扣後金額
        /// </summary>
        public decimal DiscountedAmount { get; set; }

        /// <summary>
        /// 運輸方式
        /// </summary>
        public string ShippingOptionName { get; set; }

        /// <summary>
        /// 運費
        /// </summary>
        public decimal ShippingFee { get; set; }

        /// <summary>
        /// 總金額
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// 物流編號
        /// </summary>
        public string LogisticsNumber { get; set; }

        /// <summary>
        /// 備註
        /// </summary>
        public string Remark { get; set; }

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