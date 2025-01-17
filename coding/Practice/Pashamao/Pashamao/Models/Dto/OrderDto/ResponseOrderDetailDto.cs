using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pashamao.Models.Dto.OrderDto
{
    public class ResponseOrderDetailDto
    {
        public ResponseSubOrderDto order { get; set; }

        public List<ResponseSelectOrderItemDto> orderItemDtos { get; set; }

        public List<ResponseSelectOrderStateDto> orderStateDtos { get; set; }
    }

    public class ResponseSubOrderDto
    {
        public ResponseSubOrderDto(Order order)
        {
            OrderId = order.OrderId;
            MemberId = order.MemberId;
            OrderNumber = order.OrderNumber.ToString();
            State = order.State;
            RecipientName = order.RecipientName;
            Phone = order.Phone;
            Address = order.Address;
            OriginalAmount = order.OriginalAmount;
            DiscountedAmount = order.DiscountedAmount;
            ShippingOption = order.ShippingOption;
            ShippingFee = order.ShippingFee;
            TotalAmount = order.TotalAmount;
            Remark = order.Remark;
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
        public string ShippingOption { get; set; }

        /// <summary>
        /// 運費
        /// </summary>
        public decimal ShippingFee { get; set; }

        /// <summary>
        /// 總金額
        /// </summary>
        public decimal TotalAmount { get; set; }

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

    public class ResponseSelectOrderItemDto
    {
        public ResponseSelectOrderItemDto(OrderItem orderItem)
        {
            ProductName = orderItem.ProductName;
            Style = orderItem.Style;
            Quantity = orderItem.Quantity;
            Price = orderItem.Price;
        }

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

    public class ResponseSelectOrderStateDto
    {
        public ResponseSelectOrderStateDto(OrderState orderState)
        {
            OrderStateId = orderState.OrderStateId;
            State = orderState.State;
            Remark = orderState.Remark;
            CreateTime = orderState.CreateTime;
            UpdateTime = orderState.UpdateTime;
        }

        /// <summary>
        /// 訂單狀態紀錄Id
        /// </summary>
        public int OrderStateId { get; set; }

        /// <summary>
        /// 訂單狀態
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// 備註
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 創建時間
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}