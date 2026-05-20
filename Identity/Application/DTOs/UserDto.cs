public class CreateUserDto
{
    public string Username { get; set; } = "";
    public required string Email { get; set; }
    public required string Password { get; set; }
}