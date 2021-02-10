using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;

namespace TestApiDemo.Tests
{
    public abstract class TestBase<T>
    {
        protected const string URL = "http://localhost:8081/inventory";
        protected const string CONN = "Server=(localdb)\\ProjectsV13;Database=MikeDemo;Trusted_Connection=True;";

        protected readonly string CurrentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        #region Protected Methods

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
