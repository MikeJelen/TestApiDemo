using NUnit.Framework;
using System.IO;
using System.Reflection;
using System.Text.Json;
using TestApiDemo.Controllers;
using TestApiDemo.Services;

namespace TestApiDemo.Tests
{
    public class UnitTests : TestBase<UnitTests>
    {
        private readonly InventoryController _controller = new InventoryController(new InventoryDataService());
        private readonly string _currentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

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
        public void DeleteLast()
        {
            var productSql = Path.Combine(_currentDirectory, "SQL", "GetLastProduct.sql");
            var product = (ExecuteQuery(File.ReadAllText(productSql)).Tables[0].Rows[0][0]).ToString();
            _controller.Delete(product);

            var sqlFile = Path.Combine(_currentDirectory, "SQL", "GetByName.sql");
            var results = ExecuteQuery(File.ReadAllText(sqlFile).Replace("<@Name>", product)).Tables[0];
            Assert.AreEqual(results.Rows.Count, 0);
        }

        [Test]
        public void GetAllProducts()
        {
            var sqlFile = Path.Combine(_currentDirectory, "SQL", "Get.sql");
            var expected = (ExecuteQuery(File.ReadAllText(sqlFile)).Tables[0].Rows[0][0]).ToString();
            var results = JsonSerializer.Serialize(_controller.Get());
            Assert.AreEqual(results, expected);
        }

        [Test]
        public void GetProduct()
        {
            var productSql = Path.Combine(_currentDirectory, "SQL", "GetFirstProduct.sql");
            var product = (ExecuteQuery(File.ReadAllText(productSql)).Tables[0].Rows[0][0]).ToString();

            var sqlFile = Path.Combine(_currentDirectory, "SQL", "GetByName.sql");
            var expected = (ExecuteQuery(File.ReadAllText(sqlFile).Replace("<@Name>", product)).Tables[0].Rows[0][0]).ToString();
            var results = JsonSerializer.Serialize(_controller.Get(product));
            Assert.AreEqual(results, expected?.TrimStart('[').TrimEnd(']'));
        }

    }
}