using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestApiDemo.Models
{
    public class Message
    {
        public string Action { get; set; }
        public IEnumerable<Inventory> Inventories { get; set; }
    }
}
