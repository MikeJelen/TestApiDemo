using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.Json;
using TestApiDemo.Models;

namespace TestApiDemo.Tests
{
    [ExcludeFromCodeCoverage]
    public class MessageTests : TestBase<MessageTests>
    {
        [Test]
        [Category("Functional")]
        [Property("Priority", 3)]
        public void Put()
        {
            var name = CreateTestProductName();
            var inventory = new Inventory
            {
                Name = name,
                Quantity = 900,
                CreatedOn = DateTime.UtcNow
            };

            var expected = new Message
            {
                Action = "Put",
                Inventories = new List<Inventory> { inventory }
            };

            AddedProducts.Add(name);
            _ = InventoryController.Put(name, inventory);
            var result = JsonSerializer.Deserialize<Message>(ConsumeMessage());
            Assert.AreEqual(expected.Action, result.Action);
            Assert.AreEqual(expected.Inventories.ToList().Count, result.Inventories.ToList().Count);
            Assert.AreEqual(expected.Inventories.First().Name, result.Inventories.First().Name);
            Assert.AreEqual(expected.Inventories.First().Quantity, result.Inventories.First().Quantity);
            Assert.AreEqual(expected.Inventories.First().CreatedOn.ToString("G"), result.Inventories.First().CreatedOn.ToString("G"));
        }
    }
}
