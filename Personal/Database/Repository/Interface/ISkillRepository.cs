namespace Personal.Database.Repository.Interface;

using Personal.Database.Entity;
using Personal.Domain.Entity;

public interface ISkillRepository : IEntityRepository<SkillEntity>
{
    Task AddSkill(Skill newSkill, CancellationToken ct = default);
    Task AddAccomplishment(string text, List<string> skillNames, CancellationToken ct = default);
    Task AddSynonym(string name, string skillName, CancellationToken ct = default);
    Task RemoveSkill(string skillName, CancellationToken ct = default);
    Task<PagedData<Skill>> GetSkills(string search, int page, int pageSize, string sortField, bool asc, CancellationToken ct = default);
}