using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MockPashamao.Models.Dto.OrderDto
{
    public class RequestEditOrderStateDto
    {
        /// <summary>
        /// 訂單Id
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// 目標訂單狀態
        /// </summary>
        public OrderStateEnum State { get; set; }
    }
}