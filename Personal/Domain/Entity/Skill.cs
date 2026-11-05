namespace Personal.Domain.Entity;

public enum SkillType
{
    Frontend,
    Backend,
    Architecture,
    Language,
    Devops,
    Cloud,
    Testing,
    Professional,
    Hardware,
    Data,
    AI
}

public class Skill
{
    public string? Name { get; set; }
    public SkillType? Type { get; set; }
    public string? Description { get; set; }

    public ICollection<string> Accomplishments { get; set; } = [];
}