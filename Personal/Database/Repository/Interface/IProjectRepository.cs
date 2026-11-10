using Personal.Database.Entity;
using Personal.Domain.Entity;

namespace Personal.Database.Repository.Interface;

public interface IProjectRepository : IEntityRepository<ProjectEntity>
{
    Task AddProject(Project newProject, CancellationToken ct = default);
    Task RemoveProject(string projectName, CancellationToken ct = default); // possible collision with regards to where project name is same as another project's gitname. This won't occur as I am the only developer >:).
    Task EditProject(Project newProject, CancellationToken ct = default);
    Task<PagedData<Project>> GetProjects(string search, int page, int pageSize, string sortField, bool asc, CancellationToken ct = default);
}