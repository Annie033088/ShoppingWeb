using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pashamao.Models
{
    public class CreateProductStyleViewModel
    {
        public string Style { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public bool Status { get; set; }
        public string ImageUrl { get; set; }
        public string ImageName { get; set; }
    }
}