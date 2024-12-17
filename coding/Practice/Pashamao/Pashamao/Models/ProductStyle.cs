using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pashamao.Models
{
    public class ProductStyle
    {
        public int ProductStyleId { get; set; }
        public string Style { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime LastShelveEditTime
        { get; set; }
    }
}