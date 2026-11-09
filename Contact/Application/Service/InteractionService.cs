
using System.Net;
using Contact.Application.Dtos;
using Contact.Application.Service.Interface;
using DotNetEnv;
using Resend;

namespace Contact.Application.Service;

public class InteractionService(
    IApiService apiService
) : IInteractionService
{
    public async Task<InteractionResponseDto> HandleInteraction(InteractionDto interactionDto, CancellationToken cancellationToken)
    {
        var sender = "noreply@judebradbury.com";
        var myEmail = "judebb4@gmail.com"; // (my email)
    

        // Using resend. Seems to work fine, has its own rate limiting. Later on I might host an SMTP server. Google's gmail API is absolute shit.
        IResend resend = ResendClient.Create(Env.GetString("RESEND_API"));
        
        // first, send myself the notification.
        var resp = await resend.EmailSendAsync( new EmailMessage()
        {
            From = sender,
            To = myEmail,
            Subject = $"New message from: {WebUtility.HtmlEncode(interactionDto.Email)}",
            HtmlBody = $"<p>{WebUtility.HtmlEncode(interactionDto.Body)}</p>",
        }, cancellationToken);

        // if unsuccessful, let the user know.
        if(!resp.Success)
        {
            return new InteractionResponseDto
            {
                Success = false
            };
        }

        return new InteractionResponseDto
        {
            Success = true
        };
    }
}