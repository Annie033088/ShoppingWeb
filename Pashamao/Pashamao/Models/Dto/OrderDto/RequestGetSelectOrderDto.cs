using System;
using System.ComponentModel.DataAnnotations;

namespace Pashamao.Models.Dto.OrderDto
{
    public class RequestGetSelectOrderDto
    {
        /// <summary>
        /// 訂單編號
        /// </summary>
        public long? OrderNumber { get; set; }

        /// <summary>
        /// 手機末三碼
        /// </summary>
        public int? Phone { get; set; }

        /// <summary>
        /// 會員Id
        /// </summary>
        public int? MemberId { get; set; }

        /// <summary>
        /// 日期起始
        /// </summary>
        public DateTime? StartDate { get; set; }

        /// <summary>
        /// 日期截止
        /// </summary>
        public DateTime? EndDate { get; set; }

        /// <summary>
        /// 訂單狀態
        /// </summary>
        public int? Status { get; set; }

        /// <summary>
        /// 第幾頁
        /// </summary>
        [Required]
        public int Page { get; set; }
    }
}