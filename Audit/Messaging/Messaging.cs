

using Audit.Messaging.Subscribe;
using Audit.Messaging.Subscribe.Interface;
using NATS.Net;

namespace Audit.Messaging;

public static class MessagingCollectionExtensions
{
    public static IServiceCollection AddMessaging(this IServiceCollection services)
    {
        services.AddSingleton<NatsClient>();

        services.AddSingleton<IAuditSubscribe, AuditSubscribe>();

        // force eager loading of the Subscriptions
        var sp = services.BuildServiceProvider();
        sp.GetRequiredService<IAuditSubscribe>();

        return services;
    }
}