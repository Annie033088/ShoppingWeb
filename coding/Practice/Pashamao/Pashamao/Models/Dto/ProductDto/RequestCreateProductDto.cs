using System;
using System.Collections.Generic;

namespace Pashamao.Models.Dto.ProductDto
{
    public class RequestCreateProductDto
    {
        /// <summary>
        /// 商品主項目的資料
        /// </summary>
        public RequestCreateProductDetailDto ProductDetailDto { get; set; }

        /// <summary>
        /// 商品細項的資料
        /// </summary>
        public List<RequestCreateProductStyleDto> ProductStyleDto { get; set; }

        /// <summary>
        /// 展示圖片
        /// </summary>
        public List<string> DisplayImageUrl { get; set; }
    }

    public class RequestCreateProductDetailDto
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
    }

    public class RequestCreateProductStyleDto
    {
        /// <summary>
        /// 細項名
        /// </summary>
        public string Style { get; set; }

        /// <summary>
        /// 價格
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 數量
        /// </summary>
        public int StockQuantity { get; set; }

        /// <summary>
        /// 細項圖片
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// 上/下架狀態
        /// </summary>
        public bool Status { get; set; }
    }
}