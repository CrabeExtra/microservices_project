namespace Identity.Database.Entity;

public class UserRoleEntity
{
    public Guid UserId { get; set; }
    public Guid RoleId { get; set; }

    public UserEntity User { get; set; } = default!;
    public RoleEntity Role { get; set; } = default!;
}