using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace TestApiDemo.Models
{
    [ExcludeFromCodeCoverage]
    public class Message
    {
        public string Action { get; set; }
        public IEnumerable<Inventory> Inventories { get; set; }
    }
}
