using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text.Json;
using TestApiDemo.Enumerations;
using TestApiDemo.Exceptions;
using TestApiDemo.Models;

namespace TestApiDemo.Tests
{
    public class UnitTests : TestBase<UnitTests>
    {
        [Test]
        [Category("Functional")]
        [Property("Priority", 1)]
        public void Delete()
        {
            var newProduct = InsertProductForTest();

            if (string.IsNullOrEmpty(newProduct))
            {
                Assert.Fail($"Product {newProduct} was not inserted correctly. Test terminated.");
            }
            else
            {
                _ = InventoryController.Delete(newProduct);
                var selectSqlString = Properties.Resources.GetByName.Replace("<@Name>", newProduct);
                var results = ExecuteQuery(selectSqlString).Tables[0];
                Assert.AreEqual(0, results.Rows.Count);
            }
        }

        [Test]
        [Category("Functional")]
        [Property("Priority", 1)]
        public void GetAllProducts()
        {
            var expected = (ExecuteQuery(Properties.Resources.Get).Tables[0].Rows[0][0]).ToString();
            var results = JsonSerializer.Serialize(InventoryController.Get().Result);
            Assert.AreEqual(expected, results);
        }

        [Test]
        [Category("Functional")]
        [Property("Priority", 1)]
        public void GetProduct()
        {
            var product = (ExecuteQuery(Properties.Resources.GetFirstProduct).Tables[0].Rows[0][0]).ToString();
            var expected = (ExecuteQuery(Properties.Resources.GetByName.Replace("<@Name>", product)).Tables[0].Rows[0][0]).ToString();
            var results = JsonSerializer.Serialize(InventoryController.Get(product).Result);
            Assert.AreEqual(expected?.TrimStart('[').TrimEnd(']'), results);
        }

        [Test]
        [Category("Functional")]
        [Property("Priority", 1)]
        public void GetHighestQuantity()
        {
            var expected = (ExecuteQuery(Properties.Resources.GetHighestQuantity).Tables[0].Rows[0][0]).ToString();
            var results = JsonSerializer.Serialize(InventoryController.GetByQuantityLevel(QuantityFilter.Highest).Result);
            Assert.AreEqual(expected, results);
        }

        [Test]
        [Category("Functional")]
        [Property("Priority", 1)]
        public void GetLowestQuantity()
        {
            var expected = (ExecuteQuery(Properties.Resources.GetLowestQuantity).Tables[0].Rows[0][0]).ToString();
            var results = JsonSerializer.Serialize(InventoryController.GetByQuantityLevel(QuantityFilter.Lowest).Result);
            Assert.AreEqual(expected, results);
        }


        [Test]
        [Category("Functional")]
        [Property("Priority", 1)]
        public void GetNewest()
        {
            var expected = (ExecuteQuery(Properties.Resources.GetNewest).Tables[0].Rows[0][0]).ToString();
            var results = JsonSerializer.Serialize(InventoryController.GetByCreated(CreatedFilter.Newest).Result);
            Assert.AreEqual(expected, results);
        }

        [Test]
        [Category("Functional")]
        [Property("Priority", 1)]
        public void GetOldest()
        {
            var expected = (ExecuteQuery(Properties.Resources.GetOldest).Tables[0].Rows[0][0]).ToString();
            var results = JsonSerializer.Serialize(InventoryController.GetByCreated(CreatedFilter.Oldest).Result);
            Assert.AreEqual(expected, results);
        }

        [Test]
        [Category("Functional")]
        [Property("Priority", 1)]
        public void Post()
        {
            var counter = 0;
            var inventoryList = new List<Inventory>();
            
            do
            {
                var name = CreateTestProductName();
                inventoryList.Add(new Inventory() { Name = name, Quantity = 100, CreatedOn = DateTime.Now });
                AddedProducts.Add(name);
                counter++;

            } while (counter < POST_RECORD_COUNT);

            _ = InventoryController.Post(inventoryList);
            foreach (var item in inventoryList)
            {
                var results = ExecuteQuery(
                    Properties.Resources.GetByName.Replace("<@Name>", item.Name)).Tables[0];
                Assert.AreEqual(1, results.Rows.Count);
            }
        }

        [Test]
        [Category("Functional")]
        [Property("Priority", 1)]
        public void Put()
        {
            var name = CreateTestProductName();
            var inventory = new Inventory() { Name = name, Quantity = 900, CreatedOn = DateTime.Now };
            AddedProducts.Add(name);

            _ = InventoryController.Put(name, inventory);
            var results = ExecuteQuery(
                Properties.Resources.GetByName.Replace("<@Name>", name)).Tables[0];
            Assert.AreEqual(1, results.Rows.Count);
        }

        [Test]
        [Category("Functional")]
        [Property("Priority", 2)]
        public void PutUnmatchedNames()
        {
            const string name = "Blueberries";
            var inventory = new Inventory() { Name = "Blackberries", Quantity = 900, CreatedOn = DateTime.Now };

            Assert.Catch<BadRequestException>(
                delegate { InventoryController.Put(name, inventory); },
                $"Name ({name}) does not match the name in the input json ({inventory.Name})"
            );
        }
    }
}