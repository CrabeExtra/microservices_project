namespace Identity.Application.Service.Interface;

/// <summary>
/// Generic repository interface for entities. Contains common methods for all repositories.
/// 
/// Error handling does not exist here. Services should handle all errors and specific repository functions should be favoured for that reason.
/// </summary>
/// <typeparam name="T">Any entity type</typeparam>
public interface IEntityRepository<T>
{
    /// <summary>
    /// Generic function that gets an entity by its ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Entity</returns>
    Task<T?> GetById(Guid id);

    /// <summary>
    /// Powerful generic function that gets an entity by a specified field and value. 
    /// For most cases this should really only be used when I don't have time to implement specific functions for each entity.
    /// </summary>
    /// <param name="fieldName"></param>
    /// <param name="value"></param>
    /// <returns>Entity</returns>
    Task<T?> GetByField(string fieldName, string value);

    /// <summary>
    /// Gets a paged list of entities. Useful for listing endpoints.
    /// </summary>
    /// <param name="offset"></param>
    /// <param name="limit"></param>
    /// <returns></returns>
    Task<List<T>> GetPagedAsync(int offset, int limit);

    /// <summary>
    /// Creates a new entity in the database. The entity must have all required fields filled in, otherwise this will throw an exception.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task Create(T entity);

    /// <summary>
    /// Updates an existing entity in the database. The entity must have a valid ID and all required fields filled in, otherwise this will throw an exception.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task Update(T entity);

    /// <summary>
    /// Deletes an entity from the database by its ID.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task DeleteById(Guid id);
}

