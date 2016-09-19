using System.Configuration;
using System.Data.SqlClient;
using System.Web;

namespace refactor_me.DataAccess
{
    public class DataConnection
    {
        public static string ConnectionString;

        static DataConnection()
        {
            ConnectionString = ConfigurationManager.ConnectionStrings["DataConnectionString"].ConnectionString;
        }        

        public static SqlConnection NewConnection()
        {
            return new SqlConnection(ConnectionString);
        }
    }
}