using Identity.Messaging.Publish.Interface;
using NATS.Net;

namespace Identity.Messaging.Publish;

public class UserPublish(
    NatsClient natsClient
) : IUserPublish
{
    
}