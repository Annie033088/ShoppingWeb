namespace MockPashamao.Models
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
}