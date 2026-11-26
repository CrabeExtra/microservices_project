using Microsoft.EntityFrameworkCore;
using Personal.Database.Context;
using Personal.Database.Entity;
using Personal.Database.Exceptions;
using Personal.Database.Repository.Interface;
using Personal.Domain.Entity;

namespace Personal.Database.Repository;

public class ProjectRepository(
    AppDbContext db
) : EntityRepository<ProjectEntity>(db), IProjectRepository
{
    private static Project EntityToProject(ProjectEntity entity)
    {
        return new Project
        {
            Name = entity.Name,
            GitName = entity.GitName,
            Description = entity.Description,
            ReadMe = entity.ReadMe,
            ProjectType = entity.ProjectType
        };
    }

    private static ProjectEntity ProjectToEntity(Project project)
    {
        return new ProjectEntity
        {
            Id = Guid.NewGuid(),
            Name = project.Name,
            GitName = project.GitName,
            Description = project.Description,
            ReadMe = project.ReadMe,
            ProjectType = project.ProjectType
        };
    }

    public async Task AddProject(Project newProject, CancellationToken ct = default)
    {
        var entity = ProjectToEntity(newProject);
        await CreateEntity(entity, ct);
    }

    public async Task RemoveProject(string projectName, CancellationToken ct = default)
    {
        var entity = await GetEntityByField("Name", projectName, ct);
        if(!entity.Any()) throw new RepositoryException("Referenced project not found when removing project.");

        await DeleteEntityById(entity.First().Id, ct);
    }

    public async Task EditProject(Project project, string newName, CancellationToken ct = default)
    {
        var entity = await GetEntityByField("Name", project.Name ?? "", ct);

        if(!entity.Any()) throw new RepositoryException("Referenced project not found when modifying project.");

        var foundEntity = entity.First();

        foundEntity.Name = newName;
        foundEntity.Description = project.Description ?? foundEntity.Description;
        foundEntity.ReadMe = project.ReadMe ?? foundEntity.ReadMe;
        foundEntity.ProjectType = project.ProjectType;

        await UpdateEntity(foundEntity, ct);
    }
    public async Task<PagedData<Project>> GetProjects(string search, int page, int pageSize, string sortField, bool asc, CancellationToken ct = default)
    {
        var query = db.Projects.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var hasExactMatch = await db.Projects
                .AnyAsync(p => p.Name.ToLower() == search.ToLower());

            if (hasExactMatch)
            {
                query = query.Where(p => p.Name.ToLower() == search.ToLower());
            }
            else
            {
                var pattern = $"%{search}%";
                query = query.Where(project => EF.Functions.Like(project.Name, pattern) || EF.Functions.Like(project.GitName, pattern));
            }
        }

        query = asc
            ? query.OrderBy(skill => EF.Property<object>(skill, sortField))
            : query.OrderByDescending(skill => EF.Property<object>(skill, sortField));

        var totalCount = await query.CountAsync(ct);

        var entities = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return new PagedData<Project>(
            [.. entities.Select(EntityToProject)],
            totalCount,
            page,
            pageSize
        );
    }
}