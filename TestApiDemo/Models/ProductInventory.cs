using System;
using System.Collections.Generic;

#nullable disable

namespace TestApiDemo.Models
{
    public partial class ProductInventory
    {
        public int ProductInventoryId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime LastUpdateOn { get; set; }

        public virtual Product Product { get; set; }
    }
}
