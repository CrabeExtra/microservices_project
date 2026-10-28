using Audit.Database.Repository.Interface;
using Audit.Database.Context;
using Microsoft.EntityFrameworkCore;

namespace Audit.Database.Repository;

/// <summary>
/// Base repository for all entities. Contains common methods for all repositories.
/// </summary>
public class EntityRepository<T>(AppDbContext db) 
    : IEntityRepository<T>
    where T : class
{
    public virtual async Task<T?> GetEntityById(Guid id, CancellationToken ct = default) => await db.Set<T>().FindAsync(id, ct);
    
    public virtual async Task<IEnumerable<T>> GetEntityByField(string fieldName, string value, CancellationToken ct = default) =>
        await db.Set<T>()
            .Where(e =>
                EF.Property<string>(e, fieldName) == value)
            .ToListAsync(ct);

    public virtual async Task<List<T>> GetEntityPagedAsync(int offset, int limit, CancellationToken ct = default)
    {
        return await db.Set<T>()
            .Skip(offset)
            .Take(limit)
            .ToListAsync(ct);
    }

    public virtual async Task CreateEntity(T entity, CancellationToken ct = default)
    {
        await db.Set<T>().AddAsync(entity);
        await db.SaveChangesAsync(ct);
    }

    public virtual async Task UpdateEntity(T entity, CancellationToken ct = default)
    {
        db.Set<T>().Update(entity);
        await db.SaveChangesAsync(ct);
    }

    public virtual async Task DeleteEntityById(Guid id, CancellationToken ct = default)
    {
        var entity = await db.Set<T>().FindAsync(id, ct);

        if (entity == null)
            return;

        db.Set<T>().Remove(entity);
        await db.SaveChangesAsync(ct);
    }
    
}

