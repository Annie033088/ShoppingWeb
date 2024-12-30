using System;

namespace Pashamao.Models.Dto.Product
{
    public class ProductDetailDto
    {
        public ProductDetailDto(ProductDetail productDetail)
        {
            ProductId = productDetail.ProductId;
            CategoryId = productDetail.CategoryId;
            Name = productDetail.Name;
            Description = productDetail.Description;
            Introduction = productDetail.Introduction;
            Status = productDetail.Status;
        }

        public Guid ProductId { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Introduction { get; set; }
        public bool Status { get; set; }
    }
}