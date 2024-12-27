using System;

namespace Pashamao.Models
{
    public class EditProductStyleViewModel
    {
        public int ProductStyleId { get; set; }
        public string ProductName { get; set; }
        public string Style { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }

        /// <summary>
        /// 上架下架
        /// </summary>
        public bool Status { get; set; }
        public string ImageUrl { get; set; }
        public string ImageName { get; set; }
    }
}