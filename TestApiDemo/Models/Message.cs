using System.Collections.Generic;

namespace TestApiDemo.Models
{
    public class Message
    {
        public string Action { get; set; }
        public IEnumerable<Inventory> Inventories { get; set; }
    }
}
