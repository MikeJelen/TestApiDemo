﻿using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using TestApiDemo.Controllers;
using TestApiDemo.Messaging;
using TestApiDemo.Models;
using TestApiDemo.Services;
using TestKafka;

namespace TestApiDemo.Tests
{
    [ExcludeFromCodeCoverage]
    public abstract class TestBase<T>
    {
        protected const string NAME_PARAMETER = "<@Name>";
        protected InventoryController InventoryController;
        protected readonly List<string> AddedProducts = new List<string>();
        
        #region Setup and Breakdown

        [OneTimeSetUp]
        public void SetUp()
        {
            Initialization();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            Cleanup();
        }

        #endregion

        #region Virtual Methods

        protected virtual void Cleanup()
        {
            DeleteProductsForTest();
        }

        protected virtual void Initialization()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddScoped<IMessaging, MockMessaging>();

            var provider = services.BuildServiceProvider();
            InventoryController = new InventoryController(
                new InventoryDataService(provider.GetRequiredService<IMessaging>()));
        }

        #endregion

        #region Protected Methods

        protected static void CleanMessageQueue()
        {
            var containsMessages = true;

            do
            {
                try
                {
                    ConsumeMessage();
                }
                catch
                {
                    containsMessages = false;
                }

            } while (containsMessages);
        }

        protected static string ConsumeMessage()
        {
            return MessageHandler.ConsumeMessage(
                Properties.Resources.MessageServerUri,
                Properties.Resources.MessageTopic, Properties.Resources.MessageGroupId);
        }

        protected static string CreateTestProductName()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }

        protected IEnumerable<Inventory> CreateInventoryList()
        {
            var inventoryList = new List<Inventory>();
            var random = new Random();
            var counter = 0;

            do
            {
                var name = CreateTestProductName();
                inventoryList.Add(new Inventory
                {
                    Name = name,
                    Quantity = random.Next(1, 500),
                    CreatedOn = DateTime.UtcNow
                });
                AddedProducts.Add(name);
                counter++;

            } while (counter < int.Parse(Properties.Resources.PostRecordCount));

            return inventoryList;
        }

        protected void DeleteProductsForTest()
        {
            if (!bool.Parse(Properties.Resources.PreserveTestResults))
            {
                foreach (var productName in AddedProducts)
                {
                    _ = ExecuteQuery(Properties.Resources.DeleteProduct.Replace(NAME_PARAMETER, productName));
                }
            }
        }

        protected string InsertProductForTest()
        {
            var newProduct = CreateTestProductName();

            _ = ExecuteQuery(Properties.Resources.InsertProduct.Replace(NAME_PARAMETER, newProduct));

            var selectSqlString = Properties.Resources.GetByName.Replace(NAME_PARAMETER, newProduct);

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
            using (var connection = new SqlConnection(Properties.Resources.SqlConnection))
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

        protected static string GetJsonFromResultTable(DataTable table)
        {
            var builder = new StringBuilder();
            foreach (var row in table.Rows.Cast<DataRow>())
            {
                builder.Append(row[0]);
            }

            return builder.ToString();
        }

        #endregion
    }
}
