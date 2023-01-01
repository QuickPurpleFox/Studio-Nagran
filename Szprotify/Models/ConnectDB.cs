using System;
using ReactiveUI;
using System.Text;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Data;

/*
*nohup sqlitebrowser &
*dotnet add Szprotify package System.Data.SQLite.Core
*dotnet add Szprotify package dapper
*/

namespace Szprotify;

public class ConnectDB
{
    //constructor to connect with DB idk if this works
    SQLiteConnection connection = default!;
    public ConnectDB()
    {
        try
        {
            //Build connection string
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "../DataBase/SzprotifyDB.db;Version=3.34.1";

            // Connect to SQL
            Console.Write("Connecting to SQL Server ... ");
            connection = new SQLiteConnection(builder.ConnectionString);

            connection.Open();
            Console.WriteLine("Done.");
            //https://stackoverflow.com/questions/14171794
            
        }
        catch (SQLiteException e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    //login logic
    public bool Login(string EntryUsername, string EntryPassword)
    {
        try{
            Console.Write("logging ... ");
            string SqlLogIn = "SELECT COUNT(*) FROM Users WHERE Username = @EntryUsername AND Password = @EntryPassword";
            SQLiteCommand logincommand = new SQLiteCommand(SqlLogIn, connection);

            logincommand.Parameters.AddWithValue("@Username", EntryUsername);
            logincommand.Parameters.AddWithValue("@Password", EntryPassword);

            int count = Convert.ToInt32(logincommand.ExecuteScalar());
            Console.WriteLine("count = " + count);
            if (count == 1)
            {
                Console.WriteLine("success.");
                return true;
            }
            else
            {
                Console.WriteLine("failed.");
                return false;
            }
        }
        catch (SQLiteException e)
        {
            Console.WriteLine(e.ToString());
            return false;
        }
    }
}
