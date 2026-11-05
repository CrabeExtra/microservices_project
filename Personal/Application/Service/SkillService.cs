

using Personal.Application.Service.Interface;
using Personal.Database.Repository.Interface;
using Personal.Domain.Entity;

namespace Personal.Application.Service;

public class SkillService(
    ISkillRepository skillRepository
) : ISkillService
{
    
    public async Task AddSkill(Skill newSkill, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(newSkill);

        await skillRepository.AddSkill(newSkill, ct);
    }

    public async Task AddAccomplishment(string text, List<string> skillNames, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(text);
        ArgumentNullException.ThrowIfNull(skillNames);
        ArgumentOutOfRangeException.ThrowIfZero(skillNames.Count);

        await skillRepository.AddAccomplishment(text, skillNames, ct);
    }

    public async Task AddSynonym(string name, string skillName, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(name);
        ArgumentNullException.ThrowIfNull(skillName);

        await skillRepository.AddSynonym(name, skillName, ct);
    }

    public async Task RemoveSkill(string skillName, CancellationToken ct = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(skillName);

        await skillRepository.RemoveSkill(skillName, ct);
    }

    public async Task<PagedData<Skill>> GetSkills(
        string search,
        int page,
        int pageSize,
        string sortField,
        bool asc,
        CancellationToken ct = default)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(page, 1);
        ArgumentOutOfRangeException.ThrowIfLessThan(pageSize, 1);

        return await skillRepository.GetSkills(
            search, page, pageSize, sortField, asc, ct);
    }
}