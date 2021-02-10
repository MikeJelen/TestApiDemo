﻿using System;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace TestApiDemo.Tests
{
    public class AsyncTests : TestBase<AsyncTests>
    {
        private const int CONCURRENT_THREADS = 2;

        [Test]
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
    }
}
