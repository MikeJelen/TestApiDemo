﻿using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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

        protected static string CreateTestProductName()
        {
            return (Guid.NewGuid().ToString()).Replace("-", "");
        }

        protected void DeleteProductForTest()
        {
            if (!PRESERVE_TEST_PRODUCTS)
            {
                foreach (var productName in AddedProducts)
                {
                    _ = ExecuteQuery(Properties.Resources.DeleteProduct.Replace("<@Name>", productName));
                }
            }
        }

        protected string InsertProductForTest()
        {
            var newProduct = CreateTestProductName();

            _ = ExecuteQuery(Properties.Resources.InsertProduct.Replace("<@Name>", newProduct));

            var selectSqlString = Properties.Resources.GetByName.Replace("<@Name>", newProduct);

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
