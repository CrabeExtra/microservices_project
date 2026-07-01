namespace Audit.Domain.Entity;

public class Record
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? OldData { get; set; }
    public string? NewData { get; set; }
    public string MicroserviceName { get; set; } = string.Empty;
    public string EntityName { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string EventType { get; set; } = string.Empty; // could be enum later on.
    public Guid ReferenceId { get; set; } // ID of the changed row. Also soft locks me into using Guid for every ID field in all microservices' entities.

}