
using Audit.Domain.Entity;

namespace Audit.Application.Service.Interface;

public interface IRecordService
{
    Task<Guid> CreateRecord(Record record, CancellationToken cancellationToken);
    Task<IEnumerable<Record>> GetByField(string fieldName, string value, CancellationToken cancellationToken);
    Task<Record> GetById(Guid id, CancellationToken cancellationToken);
}