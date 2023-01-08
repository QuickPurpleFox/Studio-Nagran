namespace Szprotify;

public class UserData
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Role { get; set; }

    public UserData(string Role, int Id, string Username)
    {
        this.Role = Role;
        this.Id = Id;
        this.Username = Username;
    }
}