using System;

namespace Pashamao.Models.Dto.Product
{
    public class MainProductDto
    {
        public MainProductDto(ProductDetail productDetail)
        {
            ProductId = productDetail.ProductId;
            CategoryId = productDetail.CategoryId;
            Name = productDetail.Name;
            Status = productDetail.Status;
        }

        public Guid ProductId { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public bool Status { get; set; }
    }
}