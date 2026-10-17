
using System.Configuration;
using System.Data.SqlClient;

namespace BookStoreWebForms.Data
{
    public static class Db
    {
        public static SqlConnection GetConnection()
        {
            string connectionString =
                ConfigurationManager.ConnectionStrings["BookStoreDB"]
                    .ConnectionString;

            return new SqlConnection(connectionString);
        }
    }
}
