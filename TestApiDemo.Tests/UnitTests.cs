using System;
using NUnit.Framework;
using System.IO;
using System.Text.Json;
using TestApiDemo.Controllers;
using TestApiDemo.Services;

namespace TestApiDemo.Tests
{
    public class UnitTests : TestBase<UnitTests>
    {
        private readonly InventoryController _controller = new InventoryController(new InventoryDataService());

        #region Setup and Breakdown

        [OneTimeSetUp]
        public void Initialize()
        {   
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
        }

        #endregion 

        [Test]
        public void Delete()
        {
            var newProduct = (Guid.NewGuid().ToString()).Replace("-", "");

            var insertSqlFile = Path.Combine(CurrentDirectory, "SQL", "InsertProduct.sql");
            _ = ExecuteQuery(File.ReadAllText(insertSqlFile).Replace("<@Name>", newProduct));

            var sqlFile = Path.Combine(CurrentDirectory, "SQL", "GetByName.sql");
            var selectSqlString = File.ReadAllText(sqlFile).Replace("<@Name>", newProduct);

            var newProductVerifiedResult = ExecuteQuery(selectSqlString).Tables[0];
            if (newProductVerifiedResult.Rows.Count != 1)
            {
                Assert.Fail($"Product {newProduct} was not inserted correctly ({newProductVerifiedResult.Rows.Count} returned). Test terminated.");
            }
            else
            {
                _controller.Delete(newProduct);
                var results = ExecuteQuery(selectSqlString).Tables[0];
                Assert.AreEqual(results.Rows.Count, 0);
            }
            
        }

        [Test]
        public void GetAllProducts()
        {
            var sqlFile = Path.Combine(CurrentDirectory, "SQL", "Get.sql");
            var expected = (ExecuteQuery(File.ReadAllText(sqlFile)).Tables[0].Rows[0][0]).ToString();
            var results = JsonSerializer.Serialize(_controller.Get());
            Assert.AreEqual(results, expected);
        }

        [Test]
        public void GetProduct()
        {
            var productSql = Path.Combine(CurrentDirectory, "SQL", "GetFirstProduct.sql");
            var product = (ExecuteQuery(File.ReadAllText(productSql)).Tables[0].Rows[0][0]).ToString();

            var sqlFile = Path.Combine(CurrentDirectory, "SQL", "GetByName.sql");
            var expected = (ExecuteQuery(File.ReadAllText(sqlFile).Replace("<@Name>", product)).Tables[0].Rows[0][0]).ToString();
            var results = JsonSerializer.Serialize(_controller.Get(product));
            Assert.AreEqual(results, expected?.TrimStart('[').TrimEnd(']'));
        }

    }
}