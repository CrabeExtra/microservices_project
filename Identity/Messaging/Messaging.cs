
using Identity.Messaging.Message;
using Identity.Messaging.Message.Interface;

namespace Identity.Messaging;

public static class MessagingCollectionExtensions
{
    public static IServiceCollection AddMessaging(this IServiceCollection services)
    {
        services.AddScoped<IUserMessage, UserMessage>();

        return services;
    }
}