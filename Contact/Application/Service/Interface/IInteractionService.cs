
using Contact.Application.Dtos;

namespace Contact.Application.Service.Interface;

public interface IInteractionService
{
    Task<InteractionResponseDto> HandleInteraction(InteractionDto interactionDto, CancellationToken cancellationToken);
}