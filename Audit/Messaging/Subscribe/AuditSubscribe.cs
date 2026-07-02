using Audit.Application.Service.Interface;
using Audit.Messaging.Contract;
using Audit.Messaging.Subscribe.Interface;
using Audit.Domain.Entity;

using NATS.Client.Core;
using NATS.Net;

namespace Audit.Messaging.Subscribe;

static class AuditSubscriptionConstants
{
    static public IEnumerable<string> subjects = [
        ">" // this simply subscribes to ALL subjects.
            // Might change this later so that it subscribes to audit.>
    ];
}

public class AuditSubscribe : IAuditSubscribe
{
    private readonly NatsClient natsClient;
    private readonly ILogger<AuditSubscribe> logger;
    private readonly IServiceScopeFactory scopeFactory;

    private async Task SubscribeToSubjects()
    {
        foreach(var s in AuditSubscriptionConstants.subjects)
        {
            logger.LogInformation($"Subscribing to subject: {s}");
            await foreach (NatsMsg<BaseEventData> msg in natsClient.SubscribeAsync<BaseEventData>(s))
            {
                await HandleEvent(msg.Subject, msg.Data, CancellationToken.None);
            }
        }
    }

    public AuditSubscribe(
        NatsClient natsClient, 
        ILogger<AuditSubscribe> logger,
        IServiceScopeFactory scopeFactory
    )
    {
        this.natsClient = natsClient;
        this.logger = logger;
        this.scopeFactory = scopeFactory;

        _ = SubscribeToSubjects();
    }

    public async Task HandleEvent(string subject, BaseEventData eventData, CancellationToken ct)
    {
        var record = new Record
        {
            Id = Guid.NewGuid(),
            CreatedAt = eventData.CreatedAt,
            OldData = eventData.OldData,
            NewData = eventData.NewData,
            MicroserviceName = eventData.MicroserviceName,
            EntityName = eventData.EntityName,
            Action = eventData.Action,
            EventType = eventData.EventType,
            ReferenceId = eventData.ReferenceId
        };

        logger.LogInformation($"Received event on subject: {subject}. Creating audit {record.Id}.");

        using var scope = scopeFactory.CreateScope();

        var recordService = scope.ServiceProvider.GetRequiredService<IRecordService>();

        await recordService.CreateRecord(record, ct);
    }
}