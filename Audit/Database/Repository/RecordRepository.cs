using Audit.Domain.Entity;
using Audit.Database.Entity;
using Audit.Database.Context;
using Audit.Database.Repository.Interface;

namespace Audit.Database.Repository;

class RecordRepository (
    AppDbContext db
) : EntityRepository<RecordEntity>(db), IRecordRepository
{
    private static Record ToRecord(RecordEntity entity) => new()
    {
        Id = entity.Id,
        CreatedAt = entity.CreatedAt,
        OldData = entity.OldData, // TODO obtain object from string
        NewData = entity.NewData,
        MicroserviceName = entity.MicroserviceName,
        EntityName = entity.EntityName,
        Action = entity.Action,
        EventType = entity.EventType,
        ReferenceId = entity.ReferenceId
    };

    private static RecordEntity ToRecordEntity(Record record) => new()
    {
        Id = record.Id,
        CreatedAt = record.CreatedAt,
        OldData = record.OldData, // TODO obtain string from object
        NewData = record.NewData,
        MicroserviceName = record.MicroserviceName,
        EntityName = record.EntityName,
        Action = record.Action,
        EventType = record.EventType,
        ReferenceId = record.ReferenceId
    };

    public  async Task<IEnumerable<Record>> GetByField(string fieldName, string value, CancellationToken ct) =>
        (await GetEntityByField(fieldName, value, ct)).Select(ToRecord);

    public async Task<Record> GetById(Guid id, CancellationToken ct) =>
        ToRecord(await GetEntityById(id, ct) ?? throw new Exception($"Record with Id = {id} not found."));

    public async Task<Guid> CreateRecord(Record record, CancellationToken ct)
    {
        var entity = ToRecordEntity(record);
        await CreateEntity(entity, ct);
        return entity.Id;
    }
}
