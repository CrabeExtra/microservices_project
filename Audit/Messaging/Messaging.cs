
using Audit.Messaging.Message;
using Audit.Messaging.Message.Interface;

namespace Audit.Messaging;

public static class MessagingCollectionExtensions
{
    public static IServiceCollection AddMessaging(this IServiceCollection services)
    {
        //services.AddScoped<IUserMessage, UserMessage>();

        return services;
    }
}