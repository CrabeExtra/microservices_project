
using NATS.Net;

namespace Contact.Messaging;

public static class MessagingCollectionExtensions
{
    public static IServiceCollection AddMessaging(this IServiceCollection services)
    {
        services.AddSingleton<NatsClient>();


        // force eager loading of the Subscriptions
        var sp = services.BuildServiceProvider();

        return services;
    }
}