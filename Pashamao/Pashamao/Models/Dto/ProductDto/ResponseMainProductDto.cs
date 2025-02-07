using System;

namespace Pashamao.Models.Dto.ProductDto
{
    public class ResponseMainProductDto
    {
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="productDetail"></param>
        public ResponseMainProductDto(ProductDetail productDetail)
        {
            ProductId = productDetail.ProductId;
            CategoryId = productDetail.CategoryId;
            Name = productDetail.Name;
            Status = productDetail.Status;
        }

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
        /// 商品上/下架狀態
        /// </summary>
        public bool Status { get; set; }
    }
}