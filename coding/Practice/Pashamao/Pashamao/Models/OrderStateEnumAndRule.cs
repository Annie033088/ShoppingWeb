using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pashamao.Models
{
    public enum OrderStateEnum
    {
        /// <summary>
        /// 1:待確認
        /// </summary>
        ToBeConfirmed = 1,

        /// <summary>
        /// 2:待出貨
        /// </summary>
        ToBeShipped = 2,

        /// <summary>
        /// 3:已出貨
        /// </summary>
        Shipped = 3,

        /// <summary>
        /// 4:完成
        /// </summary>
        Finish = 4,

        /// <summary>
        /// 5:買家未取商品
        /// </summary>
        NotPickedUp = 5,

        /// <summary>
        /// 6:重新寄回
        /// </summary>
        Resend = 6,

        /// <summary>
        /// 7:取消
        /// </summary>
        Cancel = 7,

        /// <summary>
        /// 8:退貨
        /// </summary>
        Returned = 8,

        /// <summary>
        /// 9:退款
        /// </summary>
        Refund = 9
    }

    /// <summary>
    /// 訂單狀態可以轉換的狀態
    /// </summary>
    public class StateTransitionRules
    {
        public Dictionary<OrderStateEnum, List<OrderStateEnum>> TransitionRules { get; set; }

        public StateTransitionRules()
        {
            TransitionRules = new Dictionary<OrderStateEnum, List<OrderStateEnum>>()
            {
                { OrderStateEnum.ToBeConfirmed, new List<OrderStateEnum> { OrderStateEnum.ToBeShipped,  OrderStateEnum.Cancel } },
                { OrderStateEnum.ToBeShipped, new List<OrderStateEnum> { OrderStateEnum.Shipped,    OrderStateEnum.Cancel } },
                { OrderStateEnum.Shipped, new List<OrderStateEnum> { OrderStateEnum.Finish, OrderStateEnum.NotPickedUp,     OrderStateEnum.Cancel } },
                { OrderStateEnum.Finish, new List<OrderStateEnum> { OrderStateEnum.Returned } },
                { OrderStateEnum.NotPickedUp, new List<OrderStateEnum> { OrderStateEnum.Resend,     OrderStateEnum.Cancel } },
                { OrderStateEnum.Resend, new List<OrderStateEnum> { OrderStateEnum.Finish, OrderStateEnum.Cancel } },
                { OrderStateEnum.Cancel, new List<OrderStateEnum>() },
                { OrderStateEnum.Returned, new List<OrderStateEnum> { OrderStateEnum.Refund, OrderStateEnum.Cancel } },
                { OrderStateEnum.Refund, new List<OrderStateEnum>() }
            };
        }
    }
}