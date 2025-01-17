using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Pashamao.Models.Dto.OrderDto
{
    public class RequestEditOrderStateRemarkDto
    {
        /// <summary>
        /// 訂單狀態Id
        /// </summary>
        [Required]
        public int OrderStateId { get; set; }

        /// <summary>
        /// 備註
        /// </summary>
        public string Remark { get; set; }
    }
}