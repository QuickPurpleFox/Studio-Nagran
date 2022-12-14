using System;
using System.Text;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Data;

/*
*nohup sqlitebrowser &
*dotnet add Szprotify package System.Data.SQLite.Core
*dotnet add Szprotify package Citrus.Avalonia
*/

namespace Szprotify;

public class ConnectDB
{
    //constructor to connect with DB
    SQLiteConnection connection = default!;
    public ConnectDB()
    {
        try
        {
            //Build connection string
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = "../DataBase/SzprotifyDB.db";

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
        try
        {
            Console.Write("logging ... ");
            string SqlLogIn = "SELECT COUNT(*) FROM Users WHERE Username = @Username AND Password = @Password";
            SQLiteCommand logincommand = new SQLiteCommand(SqlLogIn, connection);

            logincommand.Parameters.AddWithValue("@Username", EntryUsername);
            logincommand.Parameters.AddWithValue("@Password", EntryPassword);

            int count = Convert.ToInt32(logincommand.ExecuteScalar());
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

    public bool Register(string EntryUsername, string EntryPassword)
    {
        try
        {
            Console.Write("Register ... ");
            string SqlRegister;
            int count = HowMuchUsers();
            if(count>0)
            {
                SqlRegister = "INSERT INTO Users (Username, Password, Role) VALUES (@Username, @Password, 'User')";
            }
            else if(count==0)
            {
                SqlRegister = "INSERT INTO Users (Username, Password, Role) VALUES (@Username, @Password, 'Admin')";
            }
            else
            {
                Console.WriteLine("failed.");
                return false;
            }
            SQLiteCommand registercommand = new SQLiteCommand(SqlRegister, connection);

            registercommand.Parameters.AddWithValue("@Username", EntryUsername);
            registercommand.Parameters.AddWithValue("@Password", EntryPassword);

            registercommand.ExecuteScalar();

            Console.WriteLine("success.");
            return true;
        }
        catch (SQLiteException e)
        {
            Console.WriteLine(e.ToString());
            return false;
        }
    }

    public int HowMuchUsers()
    {
        try
        {
            Console.Write("Amount of users ... ");
            string Sqlcount = "SELECT COUNT(*) FROM Users";
            SQLiteCommand CountCommand = new SQLiteCommand(Sqlcount, connection);

            int CountOfUsers = Convert.ToInt32(CountCommand.ExecuteScalar());
            Console.WriteLine(CountOfUsers);
            return CountOfUsers;
        }
        catch (SQLiteException e)
        {
            Console.WriteLine(e.ToString());
            return -1;
        }
    }

    public string getRole(string username)
    {
        try
        {
            Console.Write("Role of user ... ");
            string SqlRole = "SELECT Role FROM Users WHERE Username = @username";
            SQLiteCommand RoleCommand = new SQLiteCommand(SqlRole, connection);
            RoleCommand.Parameters.AddWithValue("@Username", username);


            string UserRole = Convert.ToString(RoleCommand.ExecuteScalar()) ?? throw new ArgumentException();
            Console.WriteLine(UserRole);
            return UserRole;
        }
        catch (SQLiteException e)
        {
            Console.WriteLine(e.ToString());
            return "User";
        }
    }

    public int getId( string username)
    {
        try
        {
            Console.Write("Id of user ... ");
            string SqlIdString = "SELECT Id FROM Users WHERE Username = @username";
            SQLiteCommand IdCommand = new SQLiteCommand(SqlIdString, connection);
            IdCommand.Parameters.AddWithValue("@Username", username);

            int UserId = Convert.ToInt32(IdCommand.ExecuteScalar());
            Console.WriteLine(UserId);
            return UserId;
        }
        catch (SQLiteException e)
        {
            Console.WriteLine(e.ToString());
            return -1;
        }
    }
}
