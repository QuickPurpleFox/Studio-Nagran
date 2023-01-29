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

    public int getAlbumIDfromName(string name)
    {
        try
        {
            string SqlAlbumString = "SELECT Album_ID FROM Albums WHERE Album_Title = @name";
            SQLiteCommand AlbumCommand = new SQLiteCommand(SqlAlbumString, connection);
            AlbumCommand.Parameters.AddWithValue("@name", name);

            int AlbumId = Convert.ToInt32(AlbumCommand.ExecuteScalar());
            return AlbumId;
        }
        catch (SQLiteException e)
        {
            Console.WriteLine(e.ToString());
            return 0;
        }
    }

    public bool deleteAlbum(int id)
    {
        try
        {
            string SqlAlbumString = "DELETE FROM Albums WHERE Album_ID = @Album_ID";
            SQLiteCommand AlbumCommand = new SQLiteCommand(SqlAlbumString, connection);
            AlbumCommand.Parameters.AddWithValue("@Album_ID", id);

            AlbumCommand.ExecuteScalar();
            SqlAlbumString ="DELETE FROM Assign_albums WHERE Album_ID = @Album_ID";
            SQLiteCommand AssignCommand = new SQLiteCommand(SqlAlbumString, connection);
            AssignCommand.Parameters.AddWithValue("@Album_ID", id);
            AssignCommand.ExecuteScalar();
            return true;
        }
        catch (SQLiteException e)
        {
            Console.WriteLine(e.ToString());
            return false;
        }
    }

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

    public void getAllAlbumsID(ref List<int> albums)
    {
        try
        {
            string SqlAlbumString = "SELECT Album_ID FROM Albums";
            SQLiteCommand AlbumCommand = new SQLiteCommand(SqlAlbumString, connection);

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

    public int getSongCount(int id)
    {
        try
        {
            string SqlAlbumString = "SELECT COUNT(*) FROM Songs WHERE Album_ID = @id";
            SQLiteCommand AlbumCommand = new SQLiteCommand(SqlAlbumString, connection);
            AlbumCommand.Parameters.AddWithValue("@id", id);

            int SongCount = Convert.ToInt32(AlbumCommand.ExecuteScalar());
            return SongCount;
        }
        catch (SQLiteException e)
        {
            Console.WriteLine(e.ToString());
            return 0;
        }
    }

//SELECT COUNT(*) FROM Songs WHERE Album_ID = @id
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

    public string getAlbumCover(int album_id)
    {
        try
        {
            string SqlAlbumCoverString = "SELECT Album_Cover FROM Albums WHERE Album_ID = @id";
            SQLiteCommand CoverCommand = new SQLiteCommand(SqlAlbumCoverString, connection);
            CoverCommand.Parameters.AddWithValue("@id", album_id);

            String AlbumCover = Convert.ToString(CoverCommand.ExecuteScalar()) ?? throw new ArgumentException();;
            return AlbumCover;
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

    public string getSongName(int song_id)
    {
        try
        {
            string SqlSongNameString = "SELECT S_Title FROM Songs WHERE Song_ID = @id";
            SQLiteCommand SongNameCommand = new SQLiteCommand(SqlSongNameString, connection);
            SongNameCommand.Parameters.AddWithValue("@id", song_id);

            String AlbumArtist = Convert.ToString(SongNameCommand.ExecuteScalar()) ?? throw new ArgumentException();;
            return AlbumArtist;
        }
        catch (SQLiteException e)
        {
            Console.WriteLine(e.ToString());
            return "NoData";
        }
    }
    
    public string getSongAlbum(int song_id)
    {
        try
        {
            string SqlSongAlbumString = "SELECT Album_ID FROM Songs WHERE Song_ID = @id";
            SQLiteCommand SongAlbumCommand = new SQLiteCommand(SqlSongAlbumString, connection);
            SongAlbumCommand.Parameters.AddWithValue("@id", song_id);

            int Album_id = Convert.ToInt32(SongAlbumCommand.ExecuteScalar());
            return getAlbumName(Album_id);
        }
        catch (SQLiteException e)
        {
            Console.WriteLine(e.ToString());
            return "NoData";
        }
    }

    public string getSongDuration(int song_id)
    {
        try
        {
            string SqlSongDurationString = "SELECT Duration FROM Songs WHERE Song_ID = @id";
            SQLiteCommand SongDurationCommand = new SQLiteCommand(SqlSongDurationString, connection);
            SongDurationCommand.Parameters.AddWithValue("@id", song_id);

            String SongDuration = Convert.ToString(SongDurationCommand.ExecuteScalar()) ?? throw new ArgumentException();;
            return SongDuration;
        }
        catch (SQLiteException e)
        {
            Console.WriteLine(e.ToString());
            return "NoData";
        }
    }


    //--------------------------------------Artists--------------------------------

    public void getArtistName(ref List<String> artists)
    {
        try
        {
            string SqlArtistString = "SELECT Stage_name FROM Authors";
            SQLiteCommand ArtistCommand = new SQLiteCommand(SqlArtistString, connection);

            var reader = ArtistCommand.ExecuteReader();
            while (reader.Read())
            {
                artists.Add((String)reader.GetString(0));
            }
        }
        catch (SQLiteException e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    public void getAllUsersName(ref List<String> users)
    {
        try
        {
            string SqlUsersString = "SELECT Username FROM Users";
            SQLiteCommand UserCommand = new SQLiteCommand(SqlUsersString, connection);

            var reader = UserCommand.ExecuteReader();
            while (reader.Read())
            {
                users.Add((String)reader.GetString(0));
            }
        }
        catch (SQLiteException e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    public bool addAlbum(string EntryCoverPath, string EntryAlbumName, string EntrySelectedArtist)
    {
        try
        {
            string SqlRegister = "INSERT INTO Albums (Album_Title, Album_Cover, Author_ID) VALUES (@EntryAlbumName, @EntryCoverPath, @EntrySelectedArtist)";
            SQLiteCommand registercommand = new SQLiteCommand(SqlRegister, connection);

            registercommand.Parameters.AddWithValue("@EntryAlbumName", EntryAlbumName);
            registercommand.Parameters.AddWithValue("@EntryCoverPath", EntryCoverPath);
            registercommand.Parameters.AddWithValue("@EntrySelectedArtist", getArtistID(EntrySelectedArtist));

            registercommand.ExecuteScalar();
            return true;
        }
        catch (SQLiteException e)
        {
            Console.WriteLine(e.ToString());
            return false;
        }
    }

    public int getArtistID(String Artist_Name)
    {
        try
        {
            string SqlArtistString = "SELECT Author_ID FROM Authors WHERE Stage_name = @name";
            SQLiteCommand ArtistCommand = new SQLiteCommand(SqlArtistString, connection);
            ArtistCommand.Parameters.AddWithValue("@name", Artist_Name);

            int Author_id = Convert.ToInt32(ArtistCommand.ExecuteScalar());
            return Author_id;
        }
        catch (SQLiteException e)
        {
            Console.WriteLine(e.ToString());
            return 0;
        }
    }

    public bool assignAlbum(int album_id, int user_id)
    {
        try
        {
            string SqlRegister = "INSERT INTO Assign_albums (Album_ID, User_ID) VALUES (@EntryAlbumName, @id)";
            SQLiteCommand registercommand = new SQLiteCommand(SqlRegister, connection);

            registercommand.Parameters.AddWithValue("@EntryAlbumName", album_id);
            registercommand.Parameters.AddWithValue("@id", user_id);

            registercommand.ExecuteScalar();
            return true;
        }
        catch (SQLiteException e)
        {
            Console.WriteLine(e.ToString());
            return false;
        }
    }

    public bool deleteSong(int id)
    {
        try
        {
            string SqlSongString = "DELETE FROM Songs WHERE Song_ID = @song_ID";
            SQLiteCommand SongCommand = new SQLiteCommand(SqlSongString, connection);
            SongCommand.Parameters.AddWithValue("@song_ID", id);

            SongCommand.ExecuteScalar();
            return true;
        }
        catch (SQLiteException e)
        {
            Console.WriteLine(e.ToString());
            return false;
        }
    }

    public bool addSong(int album_id, string SongName, string SongDuration)
    {
        try
        {
            string SqlRegister = "INSERT INTO Songs (S_Title, Duration, Album_ID) VALUES (@title, @duration, @album_id)";
            SQLiteCommand registercommand = new SQLiteCommand(SqlRegister, connection);

            registercommand.Parameters.AddWithValue("@title", SongName);
            registercommand.Parameters.AddWithValue("@duration", SongDuration);
            registercommand.Parameters.AddWithValue("@album_id", album_id);

            registercommand.ExecuteScalar();
            return true;
        }
        catch (SQLiteException e)
        {
            Console.WriteLine(e.ToString());
            return false;
        }
    }

    public void changeRole(string username, string role)
    {
        try
        {
            string SqlChange = "UPDATE Users SET Role = @role WHERE Username = @username";
            SQLiteCommand changecommand = new SQLiteCommand(SqlChange, connection);

            changecommand.Parameters.AddWithValue("@username", username);
            changecommand.Parameters.AddWithValue("@role", role);

            changecommand.ExecuteScalar();
        }
        catch (SQLiteException e)
        {
            Console.WriteLine(e.ToString());
        }
    }

    public int countAdmins()
    {
        try
        {
            string SqlAdminString = "SELECT COUNT(*) FROM Users WHERE Role = 'Admin'";
            SQLiteCommand AdminCommand = new SQLiteCommand(SqlAdminString, connection);

            int AdminCount = Convert.ToInt32(AdminCommand.ExecuteScalar());
            return AdminCount;
        }
        catch (SQLiteException e)
        {
            Console.WriteLine(e.ToString());
            return 0;
        }
    }
}
