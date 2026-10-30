
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
            Subject = $"New message from: {interactionDto.Email}",
            HtmlBody = $"<p>{interactionDto.Body}</p>",
        }, cancellationToken);

        // if unsuccessful, let the user know.
        if(!resp.Success)
        {
            return new InteractionResponseDto
            {
                Success = false
            };
        }

        // send a confirmation that I have received the message.

        await resend.EmailSendAsync(new EmailMessage()
        {
            From = sender,
            To = interactionDto.Email,
            Subject = "Thank you for contacting me!",
            HtmlBody = """
                <p>Hi,</p>
                </br></br>
                <p>This is an automated message to thank you for contacting me. 
                I will review your message and get back to you shortly!</p>
                </br></br>
                <p>Kind regards,</p>
                </br></br>
                <p>Jude</p>
            """ 
        }, cancellationToken);

        return new InteractionResponseDto
        {
            Success = true
        };
    }
}