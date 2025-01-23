using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MockPashamao.Models
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
        /// 4:包裹已抵達
        /// </summary>
        PackageArrive = 4,

        /// <summary>
        /// 5:完成
        /// </summary>
        Finish = 5,

        /// <summary>
        /// 6:商品退回
        /// </summary>
        ProductReturn = 6,

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
        Returned = 9,

        /// <summary>
        /// 10:退款
        /// </summary>
        Refund = 10
    }
}