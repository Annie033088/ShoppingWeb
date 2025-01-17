using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Pashamao.Models.Dto.OrderDto
{
    public class RequestEditOrderRemarkDto
    {
        /// <summary>
        /// 訂單Id
        /// </summary>
        [Required]
        public int OrderId { get; set; }

        /// <summary>
        /// 備註
        /// </summary>
        public string Remark { get; set; }
    }
}