
using Contact.Application.Dtos;
using Contact.Application.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Contact.Api.Controller;

[ApiController]
public class InteractionController(
    IInteractionService interactionService
) : BaseController
{

    [HttpPost]
    public async Task<IActionResult> HandleInteraction([FromBody] InteractionDto interactionDto, CancellationToken cancellationToken)
    {

        //var response = await interactionService.HandleInteraction(interactionDto, cancellationToken);

        // if(response.Success)
        // {
        //     return Ok();
        // } else
        // {
        //     return BadRequest();
        // }

        return BadRequest();
        
    }
}