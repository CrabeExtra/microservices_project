namespace Identity.Database.Entity;

public class RoleEntity
{
    public Guid Id { get; set; }
    public required string Name { get; set; }

    public ICollection<UserRoleEntity> UserRoles { get; set; } = default!;
}