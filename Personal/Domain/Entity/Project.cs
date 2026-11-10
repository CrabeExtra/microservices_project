namespace Personal.Domain.Entity;

public enum ProjectType
{
    Git,
    Work,
    Other
}

public class Project
{
    public required string GitName { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? ReadMe { get; set; }
    public ProjectType ProjectType { get; set; }
}