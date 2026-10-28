
using Contact.Application.Dtos;
using Contact.Application.Service.Interface;

namespace Contact.Application.Service;

public class InteractionService() : IInteractionService
{
    public async Task<InteractionResponseDto> HandleInteraction(InteractionDto interactionDto, CancellationToken cancellationToken)
    {
        return new InteractionResponseDto
        {
            Success = true,
            Reason = ""
        };
    }
}