
using Personal.Application.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Personal.Api.Controller;

[ApiController]
public class SkillsController(
    ISkillsService skillsService
) : BaseController
{

    [HttpPost]
    public async Task<IActionResult> HandleInteraction(CancellationToken cancellationToken)
    {

        // var response = await skillsService.HandleInteraction(interactionDto, cancellationToken);

        // if(response.Success)
        // {
        //     return Ok();
        // } else
        // {
        //     return BadRequest();
        // }
        return Ok();
    }
}