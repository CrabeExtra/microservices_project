namespace Personal.Api.Dtos;

public class AddAccomplishmentDto
{
    public required string Text { get; set; }
    public required List<string> SkillNames { get; set; }
}