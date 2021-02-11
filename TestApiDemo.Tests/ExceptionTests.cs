using NUnit.Framework;
using System;
using System.Collections.Generic;
using TestApiDemo.Exceptions;
using TestApiDemo.Models;

namespace TestApiDemo.Tests
{
    public class ExceptionTests : TestBase<ExceptionTests>
    {
        [Test]
        [Category("Exception")]
        [Property("Priority", 2)]
        public void DeleteNonExistingProduct()
        {
            var name = CreateTestProductName();
            Assert.Catch<Exception>(
                delegate { InventoryController.Delete(name); },
                $"Product ({name}) not found for deletion"
            );
        }

        [Test]
        [Category("Exception")]
        [Property("Priority", 2)]
        public void PutFutureDate()
        {
            var name = CreateTestProductName();
            var inventory = new Inventory()
            {
                Name = CreateTestProductName(), 
                Quantity = 45, 
                CreatedOn = DateTime.UtcNow.AddYears(1)
            };

            Assert.Catch<BadRequestException>(
                delegate { InventoryController.Put(name, inventory); },
                $"Product {name} cannot have created on a date in the future"
            );
        }

        [Test]
        [Category("Exception")]
        [Property("Priority", 2)]
        public void PutUnmatchedNames()
        {
            var name = CreateTestProductName();
            var inventory = new Inventory()
            {
                Name = CreateTestProductName(), 
                Quantity = 900, 
                CreatedOn = DateTime.UtcNow
            };

            Assert.Catch<BadRequestException>(
                delegate { InventoryController.Put(name, inventory); },
                $"Name {name} does not match the name in the input json ({inventory.Name})"
            );
        }

        [Test]
        [Category("Exception")]
        [Property("Priority", 2)]
        public void PostZeroQuantity()
        {
            var name = CreateTestProductName();
            var inventory = new List<Inventory>()
            {
                new Inventory()
                {
                    Name = name, 
                    Quantity = -1, 
                    CreatedOn = DateTime.UtcNow
                }
            };

            Assert.Catch<BadRequestException>(
                delegate { InventoryController.Post(inventory); },
                $"Product {name} must have a quantity greater than or equal to 0"
            );
        }
    }
}
