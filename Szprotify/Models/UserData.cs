namespace Szprotify;

public class UserData
{
    public int Id { get; set; } = default!;
    public string Username { get; set; } = default!;
    public string Role { get; set; } = default!;

    public UserData(string Role, int Id)
    {
        this.Role = Role;
        this.Id = Id;
    }
}