namespace Personal.Application.Service.Interface;

using Personal.Domain.Entity;

public interface ISkillService
{
    Task AddSkill(Skill newSkill, CancellationToken ct = default);
    Task RemoveSkill(string skillName, CancellationToken ct = default);
    Task<PagedData<Skill>> GetSkills(
        string search,
        int page,
        int pageSize,
        string sortField,
        bool asc,
        CancellationToken ct = default);
}