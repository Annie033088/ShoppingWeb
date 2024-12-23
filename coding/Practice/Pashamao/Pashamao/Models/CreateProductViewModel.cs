using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pashamao.Models
{
    public class CreateProductViewModel
    {
       public int CategoryId {  get; set; }
       public string ProductName { get; set; }
        public string Description { get; set; }
        public string Introduction { get; set; }
        public bool ProductStatus { get; set; }
        public string Style { get; set; }
        public decimal Price {  get; set; }
        public int StockQuantity { get; set; }
        public bool StyleStatus { get; set; }
        public string ImageUrl { get; set; }
    }
}