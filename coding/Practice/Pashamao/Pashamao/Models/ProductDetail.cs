using System;

namespace Pashamao.Models
{
    public class ProductDetail
    {
        /// <summary>
        /// 商品Id
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// 分類Id
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// 商品名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 商品描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 商品介紹
        /// </summary>
        public string Introduction { get; set; }

        /// <summary>
        /// 商品狀態
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 商品創建時間
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 商品最後修改時間
        /// </summary>
        public DateTime LastEditTime
        { get; set; }
    }
}