
using Activity.Messaging.Message;
using Activity.Messaging.Message.Interface;

namespace Activity.Messaging;

public static class MessagingCollectionExtensions
{
    public static IServiceCollection AddMessaging(this IServiceCollection services)
    {
        services.AddScoped<INotificationMessage, NotificationMessage>();

        return services;
    }
}