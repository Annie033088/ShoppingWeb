using System;
using System.ComponentModel.DataAnnotations;

namespace Pashamao.Models.Dto.ProductDto
{
    public class RequestEditProductDetailDto
    {
        /// <summary>
        /// 商品Id
        /// </summary>
        [Required]
        public Guid ProductId { get; set; }

        /// <summary>
        /// 分類Id
        /// </summary>
        [Required]
        public int CategoryId { get; set; }

        /// <summary>
        /// 商品名稱
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 商品描述
        /// </summary>
        [Required]
        public string Description { get; set; }

        /// <summary>
        /// 商品介紹
        /// </summary>
        [Required]
        public string Introduction { get; set; }

        /// <summary>
        /// 商品狀態
        /// </summary>
        [Required]
        public bool Status { get; set; }

        /// <summary>
        /// 商品最後修改時間
        /// </summary>
        [Required]
        public DateTime LastEditTime
        { get; set; }
    }
}