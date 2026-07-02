using Audit.Messaging.Contract;

namespace Audit.Messaging.Subscribe.Interface;

public interface IAuditSubscribe
{
    Task HandleEvent(string subject, BaseEventData eventData, CancellationToken ct);
}