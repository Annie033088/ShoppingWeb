namespace Pashamao.Models.Dto.Product
{
    public class ProductStyleDto
    {
        public ProductStyleDto(ProductStyle productStyle)
        {
            ProductStyleId = productStyle.ProductStyleId;
            Style = productStyle.Style;
            Price = productStyle.Price;
            StockQuantity = productStyle.StockQuantity;
            ImageUrl = productStyle.ImageUrl;
            Status = productStyle.Status;
        }
        public int ProductStyleId { get; set; }
        public string Style { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string ImageUrl { get; set; }
        public bool Status { get; set; }
    }
}