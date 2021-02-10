using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using TestApiDemo.Controllers;
using TestApiDemo.Services;

namespace TestApiDemo.Tests
{
    public abstract class TestBase<T>
    {
        protected const bool PRESERVE_TEST_PRODUCTS = false;
        protected const string URL = "http://localhost:8081/inventory";
        protected const string CONN = "Server=(localdb)\\ProjectsV13;Database=MikeDemo;Trusted_Connection=True;";
        protected const int POST_RECORD_COUNT = 5;

        protected readonly string CurrentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        protected readonly InventoryController InventoryController = new InventoryController(new InventoryDataService());
        protected readonly List<string> AddedProducts = new List<string>();

        #region Setup and Breakdown

        [OneTimeSetUp]
        public void Initialize()
        {
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            DeleteProductForTest();
        }

        #endregion

        #region Protected Methods

        protected void DeleteProductForTest()
        {
            if (!PRESERVE_TEST_PRODUCTS)
            {
                var deleteSql = File.ReadAllText(Path.Combine(CurrentDirectory, "SQL", "DeleteProduct.sql"));
                foreach (var productName in AddedProducts)
                {
                    _ = ExecuteQuery(deleteSql.Replace("<@Name>", productName));
                }
            }
        }

        protected string InsertProductForTest()
        {
            var newProduct = (Guid.NewGuid().ToString()).Replace("-", "");

            var insertSqlFile = Path.Combine(CurrentDirectory, "SQL", "InsertProduct.sql");
            _ = ExecuteQuery(File.ReadAllText(insertSqlFile).Replace("<@Name>", newProduct));

            var sqlFile = Path.Combine(CurrentDirectory, "SQL", "GetByName.sql");
            var selectSqlString = File.ReadAllText(sqlFile).Replace("<@Name>", newProduct);

            var newProductVerifiedResult = ExecuteQuery(selectSqlString).Tables[0];
            if (newProductVerifiedResult.Rows.Count != 1)
            {
                return null;
            }

            AddedProducts.Add(newProduct);
            return newProduct;
        }

        protected static DataSet ExecuteQuery(string query)
        {
            var dataSet = new DataSet();
            using (var connection = new SqlConnection(CONN))
            {
                using (var command = new SqlCommand(query, connection))
                {
                    using (var adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(dataSet);
                    }
                }
            }

            return dataSet;
        }

        #endregion
    }
}
