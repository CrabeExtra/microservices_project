namespace Identity.Application.DTOs;

public class CreateUserDto
{
    public string Username { get; set; } = "";
    public required string Email { get; set; }
    public required string Password { get; set; }
}

public class LoginDto
{
    public string Username { get; set; } = "";
    public string Email { get; set; } = "";
    public required string Password { get; set; }
}

public class LoginResponseDto
{
    public required string Token { get; set; }
    public required string Email { get; set; }
    public required IEnumerable<string> Roles { get; set; }
}