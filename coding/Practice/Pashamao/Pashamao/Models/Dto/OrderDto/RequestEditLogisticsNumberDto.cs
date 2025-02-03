using System.ComponentModel.DataAnnotations;

namespace Pashamao.Models.Dto.OrderDto
{
    public class RequestEditLogisticsNumberDto
    {
        /// <summary>
        /// 訂單Id
        /// </summary>
        [Required]
        public int OrderId { get; set; }

        /// <summary>
        /// 物流編號
        /// </summary>
        [RegularExpression("^[a-z0-9A-Z]{0,20}$")]
        public string LogisticsNumber { get; set; }
    }
}