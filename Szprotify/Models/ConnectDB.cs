using System;
using System.Text;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Data;
using System.Collections.Generic;

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

    public int getCash( string username)
    {
        try
        {
            Console.Write("User Cash amount ... ");
            string SqlIdString = "SELECT Cash FROM Users WHERE Username = @username";
            SQLiteCommand IdCommand = new SQLiteCommand(SqlIdString, connection);
            IdCommand.Parameters.AddWithValue("@Username", username);

            int UserCash = Convert.ToInt32(IdCommand.ExecuteScalar());
            Console.WriteLine(UserCash);
            return UserCash;
        }
        catch (SQLiteException e)
        {
            Console.WriteLine(e.ToString());
            return -1;
        }
    }


    //---------------------------------------ALBUMS---------------------------------------

    public void getAlbumID(int id, ref List<int> albums)
    {
        try
        {
            string SqlAlbumString = "SELECT Album_ID FROM Assign_albums WHERE User_ID = @id";
            SQLiteCommand AlbumCommand = new SQLiteCommand(SqlAlbumString, connection);
            AlbumCommand.Parameters.AddWithValue("@id", id);

            var reader = AlbumCommand.ExecuteReader();
            while (reader.Read())
            {
                albums.Add((int)reader.GetInt32(0));
            }
        }
        catch (SQLiteException e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    public string getAlbumName(int album_id)
    {
        try
        {
            string SqlAlbumNameString = "SELECT Album_Title FROM Albums WHERE Album_ID = @id";
            SQLiteCommand IdCommand = new SQLiteCommand(SqlAlbumNameString, connection);
            IdCommand.Parameters.AddWithValue("@id", album_id);

            string AlbumName = Convert.ToString(IdCommand.ExecuteScalar()) ?? throw new ArgumentException();;
            return AlbumName;
        }
        catch (SQLiteException e)
        {
            Console.WriteLine(e.ToString());
            return "NoData";
        }
    }

    public string getAlbumArtist_id(int album_id)
    {
        try
        {
            string SqlAlbumArtistString = "SELECT Author_ID FROM Albums WHERE Album_ID = @id";
            SQLiteCommand ArtistCommand = new SQLiteCommand(SqlAlbumArtistString, connection);
            ArtistCommand.Parameters.AddWithValue("@id", album_id);

            int AlbumArtist_id = Convert.ToInt32(ArtistCommand.ExecuteScalar());
            return getAlbumArtist(AlbumArtist_id);
        }
        catch (SQLiteException e)
        {
            Console.WriteLine(e.ToString());
            return "NoData";
        }
    }

    public string getAlbumArtist(int albumartist_id)
    {
        try
        {
            string SqlAlbumArtistString = "SELECT Stage_name FROM Authors WHERE Author_ID = @id";
            SQLiteCommand ArtistCommand = new SQLiteCommand(SqlAlbumArtistString, connection);
            ArtistCommand.Parameters.AddWithValue("@id", albumartist_id);

            String AlbumArtist = Convert.ToString(ArtistCommand.ExecuteScalar()) ?? throw new ArgumentException();;
            return AlbumArtist;
        }
        catch (SQLiteException e)
        {
            Console.WriteLine(e.ToString());
            return "NoData";
        }
    }

    //---------------------------------------SONGS---------------------------------------

    public void getSongID(int album_id, ref List<int> songs)
    {
        try
        {
            string SqlSongString = "SELECT Song_ID FROM Songs WHERE Album_ID = @id";
            SQLiteCommand SongCommand = new SQLiteCommand(SqlSongString, connection);
            SongCommand.Parameters.AddWithValue("@id", album_id);

            var reader = SongCommand.ExecuteReader();
            while (reader.Read())
            {
                songs.Add((int)reader.GetInt32(0));
            }
        }
        catch (SQLiteException e)
        {
            Console.WriteLine(e.ToString());
        }
    }
}
