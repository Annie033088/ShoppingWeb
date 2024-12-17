using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pashamao.Models
{
    public class ProductSimple
    {
        public int ProductId { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public string ImageUrl { get; set; }
    }
}