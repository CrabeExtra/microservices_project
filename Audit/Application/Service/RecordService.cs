
using Audit.Application.Service.Interface;
using Audit.Database.Repository.Interface;
using Audit.Domain.Entity;

namespace Audit.Application.Service;

public class RecordService(
    IRecordRepository recordRepository
) : IRecordService
{
    public async Task<Guid> CreateRecord(Record record, CancellationToken cancellationToken) => await recordRepository.CreateRecord(record, cancellationToken);
    public async Task<IEnumerable<Record>> GetByField(string fieldName, string value, CancellationToken cancellationToken) => await recordRepository.GetByField(fieldName, value, cancellationToken);
    public async Task<Record> GetById(Guid id, CancellationToken cancellationToken) => await recordRepository.GetById(id, cancellationToken);
}