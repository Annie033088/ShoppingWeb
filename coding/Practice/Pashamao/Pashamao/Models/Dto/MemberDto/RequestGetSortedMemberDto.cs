using System.ComponentModel.DataAnnotations;

namespace Pashamao.Models.Dto.MemberDto
{
    public class RequestGetSortedMemberDto
    {
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