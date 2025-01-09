using System;
using System.Collections.Generic;

namespace Pashamao.Models.Dto.ProductDto
{
    public class ResponseProductDetailDto
    {
        /// <summary>
        /// 商品資料
        /// </summary>
        public ResponseSelectProductDetailDto SelectProductDetailDto { get; set; }

        /// <summary>
        /// 商品細項
        /// </summary>
        public List<ResponseSelectProductStyleDto> SelectProductStyleDto { get; set; }

        /// <summary>
        /// 商品展示圖片
        /// </summary>
        public List<ProductImage> SelectProductImages { get; set; }
    }

    public class ResponseSelectProductDetailDto
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="productDetail"></param>
        public ResponseSelectProductDetailDto(ProductDetail productDetail)
        {
            ProductId = productDetail.ProductId;
            CategoryId = productDetail.CategoryId;
            Name = productDetail.Name;
            Description = productDetail.Description;
            Introduction = productDetail.Introduction;
            Status = productDetail.Status;
            LastEditTime = productDetail.LastEditTime;
        }

        /// <summary>
        /// 產品Id
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
        /// 最後修改時間
        /// </summary>
        public DateTime LastEditTime
        { get; set; }
    }

    public class ResponseSelectProductStyleDto
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="productStyle"></param>
        public ResponseSelectProductStyleDto(ProductStyle productStyle)
        {
            ProductStyleId = productStyle.ProductStyleId;
            Style = productStyle.Style;
            Price = productStyle.Price;
            StockQuantity = productStyle.StockQuantity;
            ImageUrl = productStyle.ImageUrl;
            Status = productStyle.Status;
        }

        /// <summary>
        /// 細項Id
        /// </summary>
        public int ProductStyleId { get; set; }

        /// <summary>
        /// 細項名稱
        /// </summary>
        public string Style { get; set; }

        /// <summary>
        /// 價格
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 庫存量
        /// </summary>
        public int StockQuantity { get; set; }

        /// <summary>
        /// 對應圖片
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// 上/下架狀態
        /// </summary>
        public bool Status { get; set; }
    }
}