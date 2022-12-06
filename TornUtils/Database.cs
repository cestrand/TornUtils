using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TornUtils;
internal class Database
{
    public static SqlConnection GetDatabaseConnection()
    {
        SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
        
        builder.DataSource = System.Environment.GetEnvironmentVariable("DB_DATASOURCE");
        builder.UserID = System.Environment.GetEnvironmentVariable("DB_USERID");
        builder.Password = System.Environment.GetEnvironmentVariable("DB_PASSWORD");
        builder.InitialCatalog = System.Environment.GetEnvironmentVariable("DB_INITIALCATALOG");

        return new SqlConnection(builder.ConnectionString);
    }
}
