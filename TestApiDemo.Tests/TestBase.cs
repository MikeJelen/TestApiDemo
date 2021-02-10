using System.Data;
using System.Data.SqlClient;

namespace TestApiDemo.Tests
{
    public abstract class TestBase<T>
    {
        protected const string URL = "http://localhost:8081";
        protected const string CONN = "Server=(localdb)\\ProjectsV13;Database=MikeDemo;Trusted_Connection=True;";

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
