using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Pashamao.Models.Dto.OrderDto
{
    public class RequestEditOrderStateDto
    {
        /// <summary>
        /// 訂單Id
        /// </summary>
        [Required]
        public int OrderId { get; set; }

        /// <summary>
        /// 原訂單狀態
        /// </summary>
        [Required]
        public OrderStateEnum OriginalState { get; set; }

        /// <summary>
        /// 修改後訂單狀態
        /// </summary>
        [Required]
        public OrderStateEnum SelectedState { get; set; }

        /// <summary>
        /// 最後修改時間
        /// </summary>
        [Required]
        public DateTime UpdateTime { get; set; }
    }
}