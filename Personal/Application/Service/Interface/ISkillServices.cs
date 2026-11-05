namespace Personal.Application.Service.Interface;

using Personal.Domain.Entity;
/// <summary>
/// These are self-explanatory functions, no need for me to comment on their functionality.
/// 
/// I will add 'remove' functions for accomplishments and synonyms if required. I won't make mistakes while writing these.
/// If I make any mistakes I can cascade delete and recreate the skill. If this becomes a QOL issue I will add remove functions.
/// </summary>
public interface ISkillService
{
    Task AddSkill(Skill newSkill, CancellationToken ct = default);
    Task AddAccomplishment(string text, List<string> skillNames, CancellationToken ct = default);
    Task AddSynonym(string name, string skillName, CancellationToken ct = default);
    Task RemoveSkill(string skillName, CancellationToken ct = default);
    Task<PagedData<Skill>> GetSkills(
        string search,
        int page,
        int pageSize,
        string sortField,
        bool asc,
        CancellationToken ct = default);
}