using Personal.Database.Context;
using Personal.Database.Entity;
using Personal.Database.Repository.Interface;

namespace Personal.Database.Repository;

public class SkillRepository(
    AppDbContext db
) : EntityRepository<ProjectEntity>(db), IProjectRepository
{
    
}