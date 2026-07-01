using Audit.Database.Entity;
using Audit.Domain.Entity;

namespace Audit.Database.Repository.Interface;

public interface IRecordRepository
{
    Task<IEnumerable<Record>> GetByField(string fieldName, string value, CancellationToken ct);
    Task<Record> GetById(Guid id, CancellationToken ct);
    Task<Guid> CreateRecord(Record record, CancellationToken ct);
}