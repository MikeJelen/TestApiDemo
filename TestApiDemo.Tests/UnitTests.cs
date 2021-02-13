using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using TestApiDemo.Enumerations;
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
                var selectSqlString = Properties.Resources.GetByName.Replace(NAME_PARAMETER, newProduct);
                var results = ExecuteQuery(selectSqlString).Tables[0];
                Assert.AreEqual(0, results.Rows.Count);
            }
        }

        [Test]
        [Category("Functional")]
        [Property("Priority", 1)]
        public void GetAllProducts()
        {
            var expected = GetJsonFromResultTable(ExecuteQuery(Properties.Resources.Get).Tables[0]);
            var results = JsonSerializer.Serialize(InventoryController.Get().Result);
            Assert.AreEqual(expected, results);
        }

        [Test]
        [Category("Functional")]
        [Property("Priority", 1)]
        public void GetProduct()
        {
            var product = GetJsonFromResultTable(ExecuteQuery(Properties.Resources.GetFirstProduct).Tables[0]);
            var expected = GetJsonFromResultTable(ExecuteQuery(Properties.Resources.GetByName.Replace(NAME_PARAMETER, product)).Tables[0]);
            var results = JsonSerializer.Serialize(InventoryController.Get(product).Result);
            Assert.AreEqual(expected?.TrimStart('[').TrimEnd(']'), results);
        }

        [Test]
        [Category("Functional")]
        [Property("Priority", 1)]
        public void GetHighestQuantity()
        {
            var expected = GetJsonFromResultTable(ExecuteQuery(Properties.Resources.GetHighestQuantity).Tables[0]);
            var results = JsonSerializer.Serialize(InventoryController.GetByQuantityLevel(QuantityFilter.Highest).Result);
            Assert.AreEqual(expected, results);
        }

        [Test]
        [Category("Functional")]
        [Property("Priority", 1)]
        public void GetLowestQuantity()
        {
            var expected = GetJsonFromResultTable(ExecuteQuery(Properties.Resources.GetLowestQuantity).Tables[0]);
            var results = JsonSerializer.Serialize(InventoryController.GetByQuantityLevel(QuantityFilter.Lowest).Result);
            Assert.AreEqual(expected, results);
        }


        [Test]
        [Category("Functional")]
        [Property("Priority", 1)]
        public void GetNewest()
        {
            var expected = GetJsonFromResultTable(ExecuteQuery(Properties.Resources.GetNewest).Tables[0]);
            var results = JsonSerializer.Serialize(InventoryController.GetByCreated(CreatedFilter.Newest).Result);
            Assert.AreEqual(expected, results);
        }

        [Test]
        [Category("Functional")]
        [Property("Priority", 1)]
        public void GetOldest()
        {
            var expected = GetJsonFromResultTable(ExecuteQuery(Properties.Resources.GetOldest).Tables[0]);
            var results = JsonSerializer.Serialize(InventoryController.GetByCreated(CreatedFilter.Oldest).Result);
            Assert.AreEqual(expected, results);
        }

        [Test]
        [Category("Functional")]
        [Property("Priority", 1)]
        public void Post()
        {
            var inventoryList = CreateInventoryList().ToList();
            _ = InventoryController.Post(inventoryList);
            foreach (var item in inventoryList)
            {
                var results = ExecuteQuery(
                    Properties.Resources.GetByName.Replace(NAME_PARAMETER, item.Name)).Tables[0];
                Assert.AreEqual(1, results.Rows.Count);
            }
        }

        [Test]
        [Category("Functional")]
        [Property("Priority", 1)]
        public void Put()
        {
            var name = CreateTestProductName();
            var inventory = new Inventory
            {
                Name = name, 
                Quantity = 900, 
                CreatedOn = DateTime.UtcNow
            };
            AddedProducts.Add(name);

            _ = InventoryController.Put(name, inventory);
            var results = ExecuteQuery(
                Properties.Resources.GetByName.Replace(NAME_PARAMETER, name)).Tables[0];
            Assert.AreEqual(1, results.Rows.Count);
        }

        [Test]
        [Category("Functional")]
        [Property("Priority", 1)]
        public void PutSameProductDifferentCriteria()
        {
            var counter = 0;
            var quantity = 100;
            var inventoryList = new List<Inventory>();
            var name = CreateTestProductName();

            var postRecordCount = int.Parse(Properties.Resources.PostRecordCount);
            var negative = postRecordCount * -1;
            var createdOn = DateTime.UtcNow.AddDays(negative);
            AddedProducts.Add(name);

            do
            {
                quantity += counter;
                inventoryList.Add(new Inventory
                {
                    Name = name,
                    Quantity = quantity,
                    CreatedOn = createdOn.AddDays(counter)
                });

                counter++;

            } while (counter < postRecordCount);

            foreach (var inventoryItem in inventoryList)
            {
                _ = InventoryController.Put(name, inventoryItem);
            }

            var inventory = JsonSerializer.Deserialize<Inventory>(GetJsonFromResultTable(ExecuteQuery(
                Properties.Resources.GetByName.Replace(NAME_PARAMETER, name)).Tables[0]).TrimStart('[').TrimEnd(']'));

            //Check that the date is the original date
            Assert.AreEqual(createdOn.ToString("G"), inventory.CreatedOn.ToString("G"));

            //Check that the quantity is the last quantity
            Assert.AreEqual(quantity, inventory.Quantity);
        }
    }
}