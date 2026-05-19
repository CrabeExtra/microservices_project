namespace Identity.Database.Entity;

public class UserEntity
{
    public int Id { get; set; }
    public string Username { get; set; } = "";
    public string Email { get; set; } = "";
}