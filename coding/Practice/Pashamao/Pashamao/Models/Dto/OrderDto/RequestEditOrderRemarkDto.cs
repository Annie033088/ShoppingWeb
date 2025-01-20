using System.ComponentModel.DataAnnotations;

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