using Identity.Messaging.Contract;
namespace Identity.Messaging.Publish.Interface;

public interface IUserPublish
{
    Task PublishGenericEventAsync(string subject, BaseEventData eventData, CancellationToken ct);
    Task PublishUserCreatedAsync(CreateUserEventData userData, CancellationToken ct);
    Task PublishUserUpdatedAsync(UpdateUserEventData userData, CancellationToken ct);
    Task PublishUserDeletedAsync(DeleteUserEventData userData, CancellationToken ct);
}