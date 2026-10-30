
using Contact.Application.Dtos;
using Contact.Application.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Contact.Api.Controller;

[ApiController]
[Route("api/interaction")]
public class RecordController(
    IInteractionService interactionService
) : BaseController
{

    [HttpPost]
    public async Task<IActionResult> CreateRecord([FromBody] InteractionDto interactionDto, CancellationToken cancellationToken)
    {

        var response = await interactionService.HandleInteraction(interactionDto, cancellationToken);

        if(response.Success)
        {
            return Ok();
        } else
        {
            return BadRequest();
        }
        
    }
}