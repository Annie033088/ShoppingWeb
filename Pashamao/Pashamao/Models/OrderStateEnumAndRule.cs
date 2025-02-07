using System.Collections.Generic;

namespace Pashamao.Models
{
    public enum OrderStateEnum
    {
        /// <summary>
        /// 1:待確認
        /// </summary>
        ToBeConfirmed = 1,

        /// <summary>
        /// 2:申請取消
        /// </summary>
        ApplyForCancel = 2,

        /// <summary>
        /// 3:待出貨
        /// </summary>
        ToBeShipped = 3,

        /// <summary>
        /// 4:已出貨
        /// </summary>
        Shipped = 4,

        /// <summary>
        /// 5:包裹已抵達
        /// </summary>
        PackageArrive = 5,

        /// <summary>
        /// 6:完成
        /// </summary>
        Finish = 6,

        /// <summary>
        /// 7:取消
        /// </summary>
        Cancel = 7,

        /// <summary>
        /// 8:申請退貨
        /// </summary>
        ApplyForReturn = 8,

        /// <summary>
        /// 9:退貨
        /// </summary>
        returning = 9,

        /// <summary>
        /// 10:退款
        /// </summary>
        Refund = 10
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
                { OrderStateEnum.ToBeConfirmed, new List<OrderStateEnum> { OrderStateEnum.ToBeShipped, OrderStateEnum.Cancel } },
                { OrderStateEnum.ApplyForCancel, new List<OrderStateEnum> { OrderStateEnum.ToBeConfirmed, OrderStateEnum.Cancel } },
                { OrderStateEnum.ToBeShipped, new List<OrderStateEnum> { OrderStateEnum.Shipped, OrderStateEnum.Cancel } },
                { OrderStateEnum.Shipped, new List<OrderStateEnum> { OrderStateEnum.Cancel } },
                { OrderStateEnum.PackageArrive, new List<OrderStateEnum>() },
                { OrderStateEnum.Finish, new List<OrderStateEnum>() },
                { OrderStateEnum.Cancel, new List<OrderStateEnum>() },
                { OrderStateEnum.ApplyForReturn, new List<OrderStateEnum> { OrderStateEnum.returning, OrderStateEnum.PackageArrive } },
                { OrderStateEnum.returning, new List<OrderStateEnum> { OrderStateEnum.Refund, OrderStateEnum.Cancel } },
                { OrderStateEnum.Refund, new List<OrderStateEnum>() }
            };
        }
    }
}