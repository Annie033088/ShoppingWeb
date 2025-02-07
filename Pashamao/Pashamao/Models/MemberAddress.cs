namespace Pashamao.Models
{
    public class MemberAddress
    {
        /// <summary>
        /// 地址Id
        /// </summary>
        public int AddressId { get; set; }

        /// <summary>
        /// 會員Id
        /// </summary>
        public int MemberId { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 區域
        /// </summary>
        public string District { get; set; }

        /// <summary>
        /// 地址細節(ex: xx路xx巷xx號)
        /// </summary>
        public string Detail { get; set; }

        /// <summary>
        /// 郵遞區號
        /// </summary>
        public string PostalCode { get; set; }
    }
}