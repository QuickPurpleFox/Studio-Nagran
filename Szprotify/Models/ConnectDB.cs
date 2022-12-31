using System;
using ReactiveUI;
using System.Text;
using System.Data.SqlClient;

namespace Szprotify;

public class ConnectDB
{
    //constructor to connect with DB idk if this works
    public static void ConnectionToDataBase()
    {
        try
        {
            // Build connection string
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "localhost";
            builder.UserID = "admin";
            builder.Password = "admin123";
            builder.InitialCatalog = "master";

            // Connect to SQL
            Console.Write("Connecting to SQL Server ... ");
            SqlConnection connection = new SqlConnection(builder.ConnectionString);
            
            connection.Open();
            Console.WriteLine("Done.");
            
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.ToString());
        }

        Console.WriteLine("all done.");
    }
}
