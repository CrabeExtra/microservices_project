

namespace Identity.Domain.Entity;

public class User
{
    public Guid Id { get; set; }

    public string Username { get; set; } = "";
    public string Email { get; set; } = "";
    public bool EmailVerified { get; set; } = false;
    public bool PasswordSet { get; set; } = false;
    public ICollection<string> Roles { get; set; } = default!;
}