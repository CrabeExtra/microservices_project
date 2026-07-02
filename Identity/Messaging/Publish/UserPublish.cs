using Identity.Messaging.Contract;
using Identity.Messaging.Publish.Interface;
using NATS.Net;

namespace Identity.Messaging.Publish;

public class UserPublish(
    NatsClient natsClient,
    ILogger<UserPublish> logger
) : IUserPublish
{


    public async Task PublishGenericEventAsync(string subject, BaseEventData eventData, CancellationToken ct)
    {
        logger.LogInformation($"Publishing event to subject: {subject} with ReferenceId: {eventData.ReferenceId}");
        await natsClient.PublishAsync(subject, eventData);
    }

    public async Task PublishUserCreatedAsync(CreateUserEventData userData, CancellationToken ct)
    {
        await PublishGenericEventAsync("user.created", userData, ct);
    }

    public async Task PublishUserUpdatedAsync(UpdateUserEventData userData, CancellationToken ct)
    {
        await PublishGenericEventAsync("user.updated", userData, ct);
    }

    public async Task PublishUserDeletedAsync(DeleteUserEventData userData, CancellationToken ct)
    {
        await PublishGenericEventAsync("user.deleted", userData, ct);
    }
}