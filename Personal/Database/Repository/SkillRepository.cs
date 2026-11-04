

using Microsoft.EntityFrameworkCore;
using Personal.Database.Context;
using Personal.Database.Entity;
using Personal.Database.Repository;
using Personal.Database.Repository.Interface;
using Personal.Domain.Entity;

public class SkillRepository(
    AppDbContext db
) : EntityRepository<SkillEntity>(db), ISkillRepository
{
    private static Skill EntityToSkill(SkillEntity entity)
    {
        return new Skill
        {
            Name = entity.Name,
            Accomplishments = [.. entity.Accomplishments.Select(e => e.Text)],
            Description = entity.Description
        };
    }

    private static SkillEntity SkillToEntity(Skill skill)
    {
        return new SkillEntity
        {
            Id = Guid.NewGuid(),
            Name = skill.Name ?? "",
            Description = skill.Description,
            Type = (SkillType)skill.Type
        };
    }

    public async Task AddSkill(Skill newSkill, CancellationToken ct = default)
    {
        var entity = SkillToEntity(newSkill);
        await CreateEntity(entity, ct);
    }

    public async Task RemoveSkill(string skillName, CancellationToken ct = default)
    {
        // could defs turn this into a single transaction, fine for now. These are just admin controls.
        var entity = await GetEntityByField("Name", skillName, ct);

        if(!entity.Any()) return;

        await DeleteEntityById(entity.First().Id, ct); // this will only ever delete one or none. Name is a unique field.
    }

    public virtual async Task<PagedData<Skill>> GetSkills(string search, int page, int pageSize, string sortField, bool asc, CancellationToken ct = default)
    {
        var query = db.Skills.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(skill => skill.Synonyms.Any(s => EF.Functions.Like(s.Name, $"%{search}%")));
        }

        if(asc)
        {
            query = query.OrderBy(skill => EF.Property<object>(skill, sortField));
        } else
        {
            query = query.OrderByDescending(skill => EF.Property<object>(skill, sortField));
        }
        

        var totalCount = await query.CountAsync();

        var entities = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Include(skill => skill.Accomplishments)
            .ToListAsync();

        return new PagedData<Skill> (
            [.. entities.Select(EntityToSkill)],
            totalCount,
            page,
            pageSize
        );
    }
}