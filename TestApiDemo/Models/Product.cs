using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

#nullable disable

namespace TestApiDemo.Models
{
    [ExcludeFromCodeCoverage]
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
