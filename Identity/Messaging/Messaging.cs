
using Identity.Messaging.Publish;
using Identity.Messaging.Publish.Interface;
using NATS.Net;

namespace Identity.Messaging;

public static class MessagingCollectionExtensions
{
    public static IServiceCollection AddMessaging(this IServiceCollection services)
    {

        services.AddSingleton<NatsClient>();
    
        services.AddScoped<IUserPublish, UserPublish>();

        return services;
    }
}