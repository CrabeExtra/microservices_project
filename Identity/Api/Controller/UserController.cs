
using Identity.Application.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Controller;

[ApiController]
[Route("api/user")]
public class UserController(
    IUserService userService
) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDto dto, CancellationToken ct)
    {
        var userId = await userService.CreateUser(dto, ct);

        return CreatedAtAction(
            nameof(CreateUser),
            new { id = userId },
            new { id = userId }
        );
    }

    // [HttpGet("{id:guid}")]
    // public async Task<IActionResult> GetUserById(Guid id, CancellationToken ct)
    // {
    //     //var user = await userService.GetUserByIdAsync(id, ct);

    //     //return Ok(user);
    // }
}