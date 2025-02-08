namespace MockPashamao.Models.Dto.OrderDto
{
    public class RequestEditOrderStateDto
    {
        /// <summary>
        /// 訂單Id
        /// </summary>
        public int? OrderId { get; set; }

        /// <summary>
        /// 物流編號
        /// </summary>
        public string LogisticsNumber { get; set; }

        /// <summary>
        /// 目標訂單狀態
        /// </summary>
        public OrderStateEnum State { get; set; }
    }
}