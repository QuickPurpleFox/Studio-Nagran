using System;
using ReactiveUI;
using System.Text;
using System.Data.SqlClient;
using System.Data.SQLite;

/*
*nohup sqlitebrowser &
*dotnet add Szprotify package System.Data.SQLite.Core
*dotnet add Szprotify package dapper
*/

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
            builder.DataSource = "../DataBase/SzprotifyDB.db;Versin=3.34.1";

            // Connect to SQL
            Console.Write("Connecting to SQL Server ... ");
            SqlConnection connection = new SqlConnection(builder.ConnectionString);
            
            connection.Open();
            Console.WriteLine("Done.");
            //https://stackoverflow.com/questions/14171794
            connection.Close();
            
        }
        catch (SqlException e)
        {
            Console.WriteLine(e.ToString());
        }

        Console.WriteLine("all done.");
    }
}
