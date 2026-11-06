

using Microsoft.EntityFrameworkCore;
using Personal.Database.Context;
using Personal.Database.Entity;
using Personal.Database.Exceptions;
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
            Accomplishments = [.. entity.SkillToAccomplishments
                .Select(e => e.SkillAccomplishment.Text)],
            Description = entity.Description,
            Type = entity.Type
        };
    }

    private static SkillEntity SkillToEntity(Skill skill)
    {
        return new SkillEntity
        {
            Id = Guid.NewGuid(),
            Name = skill.Name ?? "",
            Description = skill.Description,
            Type = skill.Type
        };
    }

    public async Task AddSkill(Skill newSkill, CancellationToken ct = default)
    {
        var entity = SkillToEntity(newSkill);
        await CreateEntity(entity, ct);
    }

    public async Task AddAccomplishment(
        string text,
        List<string> skillNames,
        CancellationToken ct = default)
    {
        var skills = await db.Skills
            .Where(s => skillNames.Contains(s.Name))
            .ToListAsync(ct);

        var missingSkills = skillNames
            .Except(skills.Select(s => s.Name))
            .ToArray();

        if (missingSkills.Length > 0)
            throw new RepositoryException(
                $"Referenced skills not found: {string.Join(", ", missingSkills)}");

        var accomplishment = new SkillAccomplishmentEntity
        {
            Id = Guid.NewGuid(),
            Text = text
        };

        await db.SkillAccomplishments.AddAsync(accomplishment, ct);

        var relationships = skills.Select(skill =>
            new SkillToSkillAccomplishmentEntity
            {
                SkillId = skill.Id,
                SkillAccomplishmentId = accomplishment.Id
            });

        await db.SkillToSkillAccomplishments.AddRangeAsync(relationships, ct);

        await db.SaveChangesAsync(ct);
    }

    public async Task AddSynonym(string name, string skillName, CancellationToken ct = default)
    {
        var entity = await GetEntityByField("Name", skillName, ct);

        if(!entity.Any()) throw new RepositoryException("Referenced skill not found when adding synonym.");

        await db.SkillSynonyms.AddAsync(new SkillSynonymsEntity
        {
            Id = Guid.NewGuid(),
            Name = name,
            SkillId = entity.First().Id // names are unique.
        }, ct);

        await db.SaveChangesAsync(ct);
    }

    public async Task RemoveSkill(string skillName, CancellationToken ct = default)
    {
        // could defs turn this into a single transaction, fine for now. These are just admin controls.
        var entity = await GetEntityByField("Name", skillName, ct);

        if(!entity.Any()) throw new RepositoryException("Referenced skill not found when removing skill.");

        await DeleteEntityById(entity.First().Id, ct); // this will only ever delete one or none. Name is a unique field.
    }

    public virtual async Task<PagedData<Skill>> GetSkills(
        string search,
        int page,
        int pageSize,
        string sortField,
        bool asc,
        CancellationToken ct = default)
    {
        var query = db.Skills.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var hasExactMatch = await db.Skills
                .AnyAsync(skill => skill.Synonyms
                    .Any(s => s.Name.ToLower() == search.ToLower()), ct);

            if (hasExactMatch)
            {
                query = query.Where(skill => skill.Synonyms
                    .Any(s => s.Name.ToLower() == search.ToLower()));
            }
            else
            {
                var pattern = $"%{search}%";
                query = query.Where(skill => skill.Synonyms
                    .Any(s => EF.Functions.Like(s.Name, pattern)));
            }
        }

        query = asc
            ? query.OrderBy(skill => EF.Property<object>(skill, sortField))
            : query.OrderByDescending(skill => EF.Property<object>(skill, sortField));

        var totalCount = await query.CountAsync(ct);

        var entities = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Include(skill => skill.SkillToAccomplishments)
                .ThenInclude(link => link.SkillAccomplishment)
            .ToListAsync(ct);

        return new PagedData<Skill>(
            [.. entities.Select(EntityToSkill)],
            totalCount,
            page,
            pageSize
        );
    }
}