namespace Audit.Database.Entity;

public class RecordEntity
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? OldData { get; set; }
    public string? NewData { get; set; }
    public string MicroserviceName { get; set; } = string.Empty;
    public string EntityName { get; set; } = string.Empty; // combine with MicroserviceName or figure out a way of optimising the query if required.
    public string Action { get; set; } = string.Empty;
    public string EventType { get; set; } = string.Empty; // could be enum later on.
    public Guid ReferenceId { get; set; }
}