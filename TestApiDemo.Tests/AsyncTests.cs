using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TestApiDemo.Models;

namespace TestApiDemo.Tests
{
    public class AsyncTests : TestBase<AsyncTests>
    {
        [Test]
        [Category("Async")]
        [Property("Priority", 1)]
        public void Delete()
        {
            var testProducts = new List<string>();

            for (var i = 0; i < int.Parse(Properties.Resources.ConcurrentThreads); i++)
            {
                var newProduct = InsertProductForTest();

                if (string.IsNullOrEmpty(newProduct))
                {
                    Assert.Fail($"Product {newProduct} was not inserted correctly. Test terminated.");
                }

                testProducts.Add(newProduct);
            }

            var caughtExceptions = 0;
            Parallel.ForEach(testProducts, async testProduct =>
            {
                try
                {
                    var asyncResult = await InventoryController.Delete(testProduct);
                    Assert.IsTrue(asyncResult.IsSuccessful);

                    var results = ExecuteQuery(Properties.Resources.GetByName.Replace("<@Name>", testProduct)).Tables[0];
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
        public void Post()
        {
            var counter = 0;
            var inventoryList = new Dictionary<int, List<Inventory>>();

            for (var i = 0; i < int.Parse(Properties.Resources.ConcurrentThreads); i++)
            {
                var inventories = new List<Inventory>();
                do
                {
                    var name = CreateTestProductName();
                    inventories.Add(new Inventory() { Name = name, Quantity = 100, CreatedOn = DateTime.Now });
                    AddedProducts.Add(name);
                    counter++;

                } while (counter < int.Parse(Properties.Resources.PostRecordCount));

                inventoryList.Add(i, inventories);
            }


            var caughtExceptions = 0;
            Parallel.ForEach(inventoryList, async inventoryItem =>
            {
                try
                {
                    var (_, value) = inventoryItem;
                    var result = await InventoryController.Post(value);
                    Assert.IsTrue(result.IsSuccessful);

                    foreach (var item in value)
                    {
                        var results = ExecuteQuery(Properties.Resources.GetByName.Replace("<@Name>", item.Name)).Tables[0];
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
