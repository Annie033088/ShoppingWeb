using System.ComponentModel.DataAnnotations;

namespace Pashamao.Models.Dto.OrderDto
{
    public class RequestOrderIdDto
    {
        /// <summary>
        /// 訂單Id
        /// </summary>
        [Required]
        public int OrderId { get; set; }
    }
}