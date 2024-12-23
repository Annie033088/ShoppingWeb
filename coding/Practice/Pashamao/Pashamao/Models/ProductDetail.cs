using System;

namespace Pashamao.Models
{
    public class ProductDetail
    {
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Introduction { get; set; }
        public bool Status { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime LastShelveEditTime
        { get; set; }
    }
}