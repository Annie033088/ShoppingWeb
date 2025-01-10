using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        [Required]
        public int CategoryId { get; set; }

        /// <summary>
        /// 商品名稱 可空
        /// </summary>
        [RegularExpression("^.{1,30}$", ErrorMessage = "至多30字")]
        public string Name { get; set; }

        /// <summary>
        /// 商品描述 (若為空會處理為空字串)
        /// </summary>
        [RegularExpression("^.{0,40}$", ErrorMessage = "至多40字")] 
        public string Description { get; set; }

        /// <summary>
        /// 商品介紹 (若為空會處理為空字串)
        /// </summary>
        [RegularExpression("^.{0,40}$", ErrorMessage = "至多1500字")]
        public string Introduction { get; set; }

        /// <summary>
        /// 商品狀態
        /// </summary>
        [Required]
        public bool Status { get; set; }
    }

    public class RequestCreateProductStyleDto
    {
        /// <summary>
        /// 細項名 不可空
        /// </summary>
        [RegularExpression("^.{1,25}$", ErrorMessage = "至多40字")]
        public string Style { get; set; }

        /// <summary>
        /// 價格
        /// </summary>
        [Required]
        public decimal Price { get; set; }

        /// <summary>
        /// 數量
        /// </summary>
        [Required]
        public int StockQuantity { get; set; }

        /// <summary>
        /// 細項圖片
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// 上/下架狀態
        /// </summary>
        [Required]
        public bool Status { get; set; }
    }
}