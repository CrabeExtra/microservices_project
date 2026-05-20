namespace Identity.Database.Entity;

public class UserEntity
{
    public Guid Id { get; set; }

    public string Username { get; set; } = "";
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public bool EmailVerified { get; set; } = false;
    public bool PasswordSet { get; set; } = false;

    public ICollection<UserRoleEntity> UserRoles { get; set; } = default!;

}