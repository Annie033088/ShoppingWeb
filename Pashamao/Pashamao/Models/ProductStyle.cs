using System;

namespace Pashamao.Models
{
    public class ProductStyle
    {
        /// <summary>
        /// 細項Id
        /// </summary>
        public int ProductStyleId { get; set; }

        /// <summary>
        /// 商品Id
        /// </summary>
        public Guid ProductId { get; set; }

        /// <summary>
        /// 細項名
        /// </summary>
        public string Style { get; set; }

        /// <summary>
        /// 價格
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 庫存
        /// </summary>
        public int StockQuantity { get; set; }

        /// <summary>
        /// 對應圖片
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// 上/下架
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 創建時間
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 最後修改時間
        /// </summary>
        public DateTime LastShelveEditTime
        { get; set; }
    }
}