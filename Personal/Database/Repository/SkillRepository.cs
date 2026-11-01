

using Personal.Database.Context;
using Personal.Database.Entity;
using Personal.Database.Repository;
using Personal.Database.Repository.Interface;

public class SkillRepository(
    AppDbContext db
) : EntityRepository<SkillEntity>(db), ISkillRepository
{
    
}