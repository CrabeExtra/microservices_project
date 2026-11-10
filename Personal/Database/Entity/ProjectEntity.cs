using Personal.Domain.Entity;

namespace Personal.Database.Entity;


public class ProjectEntity
{
    public Guid Id { get; set; }
    public required string GitName { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? ReadMe { get; set; }

    public ProjectType ProjectType { get; set; }
}