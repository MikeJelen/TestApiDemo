using System;
using System.Diagnostics.CodeAnalysis;

namespace TestApiDemo.Models
{
    [ExcludeFromCodeCoverage]
    public class Inventory
    {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
