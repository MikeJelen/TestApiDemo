using System;
using System.Collections.Generic;

#nullable disable

namespace TestApiDemo.Models
{
    public partial class Product
    {
        public Product()
        {
            ProductInventories = new HashSet<ProductInventory>();
        }

        public int ProductId { get; set; }
        public string Name { get; set; }
        public DateTime CreatedOn { get; set; }

        public virtual ICollection<ProductInventory> ProductInventories { get; set; }
    }
}
