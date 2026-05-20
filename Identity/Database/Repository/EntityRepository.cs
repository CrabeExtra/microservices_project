using Identity.Application.Service.Interface;
using Identity.Database.Context;
using Microsoft.EntityFrameworkCore;

namespace Identity.Application.Service;

/// <summary>
/// Base repository for all entities. Contains common methods for all repositories.
/// </summary>
public class EntityRepository<T>(AppDbContext db) 
    : IEntityRepository<T>
    where T : class
{
    public async Task<T?> GetById(Guid id) => await db.Set<T>().FindAsync(id);
    
    public async Task<T?> GetByField(string fieldName, string value) =>
        await db.Set<T>()
            .FirstOrDefaultAsync(e =>
                EF.Property<string>(e, fieldName) == value);

    public async Task<List<T>> GetPagedAsync(int offset, int limit)
    {
        return await db.Set<T>()
            .Skip(offset)
            .Take(limit)
            .ToListAsync();
    }

    public async Task Create(T entity)
    {
        await db.Set<T>().AddAsync(entity);
        await db.SaveChangesAsync();
    }

    public async Task Update(T entity)
    {
        db.Set<T>().Update(entity);
        await db.SaveChangesAsync();
    }

    public async Task DeleteById(Guid id)
    {
        var entity = await db.Set<T>().FindAsync(id);

        if (entity == null)
            return;

        db.Set<T>().Remove(entity);
        await db.SaveChangesAsync();
    }
    
}

