
namespace Identity.Messaging.Contract;

public class BaseEventData
{
    public DateTime CreatedAt { get; set; }
    public string? OldData { get; set; }
    public string? NewData { get; set; }
    public string MicroserviceName { get; set; } = string.Empty;
    public string EntityName { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string EventType { get; set; } = string.Empty;
    public Guid ReferenceId { get; set; }
}