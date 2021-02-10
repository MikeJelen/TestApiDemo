using NUnit.Framework;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace TestApiDemo.Tests
{
    public class AsyncTests : TestBase<AsyncTests>
    {
        private HttpClient _client;

        #region Setup and Breakdown

        [OneTimeSetUp]
        public void Initialize()
        {
            _client= new HttpClient();
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            _client.Dispose();
        }

        #endregion

        [Test]
        public void DeleteAsync()
        {
            var productSql = Path.Combine(CurrentDirectory, "SQL", "GetLastProduct.sql");
            var product = (ExecuteQuery(File.ReadAllText(productSql)).Tables[0].Rows[0][0]).ToString();

            var sqlFile = Path.Combine(CurrentDirectory, "SQL", "GetByName.sql");
            var results = ExecuteQuery(File.ReadAllText(sqlFile).Replace("<@Name>", product)).Tables[0];
            Assert.AreEqual(results.Rows.Count, 0);
        }


        private async Task<string> GetProductAsync(string name)
        {
            var response = await _client.GetAsync($"{URL}/{name}")
                .ConfigureAwait(false);

            return await response.Content.ReadAsStringAsync();
        }

        private async Task<HttpStatusCode> DeleteProductAsync(string name)
        {
            var response = await _client.DeleteAsync($"{URL}/{name}")
                .ConfigureAwait(false);

            return response.StatusCode;
        }
    }
}
