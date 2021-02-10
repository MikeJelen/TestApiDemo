using NUnit.Framework;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace TestApiDemo.Tests
{
    public class UnitTests : TestBase<UnitTests>
    {
        [Test]
        public void Delete()
        {
            var newProduct = InsertProductForTest();

            if (string.IsNullOrEmpty(newProduct))
            {
                Assert.Fail($"Product {newProduct} was not inserted correctly. Test terminated.");
            }
            else
            {
                InventoryController.Delete(newProduct);

                var sqlFile = Path.Combine(CurrentDirectory, "SQL", "GetByName.sql");
                var selectSqlString = File.ReadAllText(sqlFile).Replace("<@Name>", newProduct);
                var results = ExecuteQuery(selectSqlString).Tables[0];


                Assert.AreEqual(0, results.Rows.Count);
            }
        }

        [Test]
        public void GetAllProducts()
        {
            var sqlFile = Path.Combine(CurrentDirectory, "SQL", "Get.sql");
            var expected = (ExecuteQuery(File.ReadAllText(sqlFile)).Tables[0].Rows[0][0]).ToString();
            var results = JsonSerializer.Serialize(InventoryController.Get().Result);
            Assert.AreEqual(expected, results);
        }

        [Test]
        public void GetProduct()
        {
            var productSql = Path.Combine(CurrentDirectory, "SQL", "GetFirstProduct.sql");
            var product = (ExecuteQuery(File.ReadAllText(productSql)).Tables[0].Rows[0][0]).ToString();

            var sqlFile = Path.Combine(CurrentDirectory, "SQL", "GetByName.sql");
            var expected = (ExecuteQuery(File.ReadAllText(sqlFile).Replace("<@Name>", product)).Tables[0].Rows[0][0]).ToString();
            var results = JsonSerializer.Serialize(InventoryController.Get(product).Result);
            Assert.AreEqual(expected?.TrimStart('[').TrimEnd(']'), results);
        }

    }
}