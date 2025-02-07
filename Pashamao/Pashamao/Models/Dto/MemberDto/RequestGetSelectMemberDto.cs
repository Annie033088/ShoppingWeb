using System.ComponentModel.DataAnnotations;

namespace Pashamao.Models.Dto.MemberDto
{
    public class RequestGetSelectMemberDto
    {
        /// <summary>
        /// 搜尋的欄位
        /// </summary>
        [Required]
        public string SelectColumn { get; set; }

        /// <summary>
        /// 搜尋的值
        /// </summary>
        [Required]
        public string Value { get; set; }

        /// <summary>
        /// 排序的欄位
        /// </summary>
        [Required]
        public string SortColumn { get; set; }

        /// <summary>
        /// 第多少頁面
        /// </summary>
        [Required]
        public int Page { get; set; }

        /// <summary>
        /// 升序或降序
        /// </summary>
        [Required]
        public string SortOrder { get; set; }
    }
}