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
        public DateTime LastShelveEditTime
        { get; set; }

        /// <summary>
        /// 上架下架
        /// </summary>
        public bool Status { get; set; }
        public string NewImageUrl { get; set; }
        public string OldImageUrl { get; set; }
        public string ImageName { get; set; }

        /// <summary>
        /// 是否有調整過上架下架(有的話就會更新上下架時間)
        /// </summary>
        public bool UpdateStyleStatus { get; set; }
    }
}