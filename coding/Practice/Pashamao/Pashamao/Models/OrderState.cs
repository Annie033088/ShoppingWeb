using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pashamao.Models
{
    public class OrderState
    {
        /// <summary>
        /// 訂單狀態紀錄Id
        /// </summary>
        public int OrderStateId { get; set; }

        /// <summary>
        /// 訂單Id
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// 訂單狀態
        /// </summary>
        public int State { get; set; }

        /// <summary>
        /// 備註
        /// </summary>
        public string Remark {  get; set; }

        /// <summary>
        /// 創建時間
        /// </summary>
        public DateTime CreateTime {  get; set; }

        /// <summary>
        /// 更新時間
        /// </summary>
        public DateTime UpdateTime { get; set; }
    }
}