using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
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

                var sqlFile = Path.Combine(CurrentDirectory, "SQL", "GetByName.sql");
                var selectSqlString = File.ReadAllText(sqlFile).Replace("<@Name>", newProduct);
                var results = ExecuteQuery(selectSqlString).Tables[0];

                Assert.AreEqual(0, results.Rows.Count);
            }
        }

        [Test]
        [Category("Functional")]
        [Property("Priority", 1)]
        public void GetAllProducts()
        {
            var sqlFile = Path.Combine(CurrentDirectory, "SQL", "Get.sql");
            var expected = (ExecuteQuery(File.ReadAllText(sqlFile)).Tables[0].Rows[0][0]).ToString();
            var results = JsonSerializer.Serialize(InventoryController.Get().Result);
            Assert.AreEqual(expected, results);
        }

        [Test]
        [Category("Functional")]
        [Property("Priority", 1)]
        public void GetProduct()
        {
            var productSql = Path.Combine(CurrentDirectory, "SQL", "GetFirstProduct.sql");
            var product = (ExecuteQuery(File.ReadAllText(productSql)).Tables[0].Rows[0][0]).ToString();

            var sqlFile = Path.Combine(CurrentDirectory, "SQL", "GetByName.sql");
            var expected = (ExecuteQuery(File.ReadAllText(sqlFile).Replace("<@Name>", product)).Tables[0].Rows[0][0]).ToString();
            var results = JsonSerializer.Serialize(InventoryController.Get(product).Result);
            Assert.AreEqual(expected?.TrimStart('[').TrimEnd(']'), results);
        }

        [Test]
        [Category("Functional")]
        [Property("Priority", 1)]
        public void GetHighestQuantity()
        {
            var sqlFile = Path.Combine(CurrentDirectory, "SQL", "GetHighestQuantity.sql");
            var expected = (ExecuteQuery(File.ReadAllText(sqlFile)).Tables[0].Rows[0][0]).ToString();
            var results = JsonSerializer.Serialize(InventoryController.GetByQuantityLevel(QuantityFilter.Highest).Result);
            Assert.AreEqual(expected, results);
        }

        [Test]
        [Category("Functional")]
        [Property("Priority", 1)]
        public void GetLowestQuantity()
        {
            var sqlFile = Path.Combine(CurrentDirectory, "SQL", "GetLowestQuantity.sql");
            var expected = (ExecuteQuery(File.ReadAllText(sqlFile)).Tables[0].Rows[0][0]).ToString();
            var results = JsonSerializer.Serialize(InventoryController.GetByQuantityLevel(QuantityFilter.Lowest).Result);
            Assert.AreEqual(expected, results);
        }


        [Test]
        [Category("Functional")]
        [Property("Priority", 1)]
        public void GetNewest()
        {
            var sqlFile = Path.Combine(CurrentDirectory, "SQL", "GetNewest.sql");
            var expected = (ExecuteQuery(File.ReadAllText(sqlFile)).Tables[0].Rows[0][0]).ToString();
            var results = JsonSerializer.Serialize(InventoryController.GetByCreated(CreatedFilter.Newest).Result);
            Assert.AreEqual(expected, results);
        }

        [Test]
        [Category("Functional")]
        [Property("Priority", 1)]
        public void GetOldest()
        {
            var sqlFile = Path.Combine(CurrentDirectory, "SQL", "GetOldest.sql");
            var expected = (ExecuteQuery(File.ReadAllText(sqlFile)).Tables[0].Rows[0][0]).ToString();
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
            var sqlQuery = File.ReadAllText(Path.Combine(CurrentDirectory, "SQL", "GetByName.sql"));

            do
            {
                var name = (Guid.NewGuid().ToString()).Replace("-", "");
                inventoryList.Add(new Inventory() { Name = name, Quantity = 100, CreatedOn = DateTime.Now });
                AddedProducts.Add(name);
                counter++;

            } while (counter < POST_RECORD_COUNT);

            _ = InventoryController.Post(inventoryList);

            foreach (var item in inventoryList)
            {
                var results = ExecuteQuery(sqlQuery.Replace("<@Name>", item.Name)).Tables[0];
                Assert.AreEqual(1, results.Rows.Count);
            }
        }

        [Test]
        [Category("Functional")]
        [Property("Priority", 1)]
        public void Put()
        {
            var name = (Guid.NewGuid().ToString()).Replace("-", "");
            var inventory = new Inventory() { Name = name, Quantity = 900, CreatedOn = DateTime.Now };
            AddedProducts.Add(name);

            _ = InventoryController.Put(name, inventory);

            var sqlFile = Path.Combine(CurrentDirectory, "SQL", "GetByName.sql");
            var results = ExecuteQuery(File.ReadAllText(sqlFile).Replace("<@Name>", name)).Tables[0];

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