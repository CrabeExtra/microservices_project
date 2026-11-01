namespace Personal.Database.Entity;

public enum SkillType
{
    Frontend,
    Backend,
    Architecture,
    Language,
    Multidisciplinary
}

public class SkillEntity
{
    public required Guid Id { get; set; }
    public string Name { get; set; } = "";
    public SkillType Type { get; set; }

    public ICollection<SkillAccomplishmentEntity> Accomplishments { get; set; } = [];
    public ICollection<SkillSynonymsEntity> Synonyms { get; set; } = [];
}

public class SkillAccomplishmentEntity
{
    public required Guid Id { get; set; }
    public string Description { get; set; } = "";
    public required Guid SkillId { get; set; }
    public SkillEntity Skill { get; set; } = null!;
}

public class SkillSynonymsEntity
{
    public required Guid Id { get; set; }
    public string Name { get; set; } = "";
    public required Guid SkillId { get; set; }
    public SkillEntity Skill { get; set; } = null!;
}