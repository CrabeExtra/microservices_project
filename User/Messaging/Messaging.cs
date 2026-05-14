
using User.Messaging.Message;
using User.Messaging.Message.Interface;

namespace User.Messaging;

public static class MessagingCollectionExtensions
{
    public static IServiceCollection AddMessaging(this IServiceCollection services)
    {
        services.AddScoped<IUserMessage, UserMessage>();

        return services;
    }
}