using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using TestApiDemo.Models;

namespace TestApiDemo.Tests
{
    public class AsyncTests : TestBase<AsyncTests>
    {
        private const int CONCURRENT_THREADS = 2;

        [Test]
        [Category("Async")]
        [Property("Priority", 1)]
        public async Task Delete()
        {
            var testProducts = new List<string>();

            for (var i = 0; i < CONCURRENT_THREADS; i++)
            {
                var newProduct = InsertProductForTest();

                if (string.IsNullOrEmpty(newProduct))
                {
                    Assert.Fail($"Product {newProduct} was not inserted correctly. Test terminated.");
                }

                testProducts.Add(newProduct);
            }

            var selectSqlString = await File.ReadAllTextAsync(Path.Combine(CurrentDirectory, "SQL", "GetByName.sql"));
            var caughtExceptions = 0;

            Parallel.ForEach(testProducts, async testProduct =>
            {
                try
                {
                    var asyncResult = await InventoryController.Delete(testProduct);
                    Assert.IsTrue(asyncResult.IsSuccessful);

                    var results = ExecuteQuery(selectSqlString.Replace("<@Name>", testProduct)).Tables[0];
                    Assert.AreEqual(0, results.Rows.Count);
                }
                catch
                {
                    caughtExceptions++;
                }

            });

            Assert.AreEqual(0, caughtExceptions);
        }

        [Test]
        [Category("Async")]
        [Property("Priority", 1)]
        public async Task Post()
        {
            var counter = 0;
            var inventoryList = new Dictionary<int, List<Inventory>>();

            for (var i = 0; i < CONCURRENT_THREADS; i++)
            {
                var inventories = new List<Inventory>();
                do
                {
                    var name = (Guid.NewGuid().ToString()).Replace("-", "");
                    inventories.Add(new Inventory() { Name = name, Quantity = 100, CreatedOn = DateTime.Now });
                    AddedProducts.Add(name);
                    counter++;

                } while (counter < POST_RECORD_COUNT);

                inventoryList.Add(i, inventories);
            }

            
            var caughtExceptions = 0;
            var selectSqlString = await File.ReadAllTextAsync(Path.Combine(CurrentDirectory, "SQL", "GetByName.sql"));

            Parallel.ForEach(inventoryList, async inventoryItem =>
            {
                try
                {
                    var (_, value) = inventoryItem;
                    var result = await InventoryController.Post(value);
                    Assert.IsTrue(result.IsSuccessful);

                    foreach (var item in value)
                    {
                        var results = ExecuteQuery(selectSqlString.Replace("<@Name>", item.Name)).Tables[0];
                        Assert.AreEqual(1, results.Rows.Count);
                    }
                }
                catch
                {
                    caughtExceptions++;
                }

            });

            Assert.AreEqual(0, caughtExceptions);
        }
    }
}
