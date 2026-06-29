
using Identity.Application.DTOs;
using Identity.Application.Exceptions;
using Identity.Application.Service.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Identity.Api.Controller;

[ApiController]
[Route("api/user")]
public class UserController(
    IUserService userService,
    IAuthService authService
) : ControllerBase
{
    [HttpPost("signup")]
    public async Task<IActionResult> SignUp([FromBody] CreateUserDto dto, CancellationToken ct)
    {
        var userId = await authService.SignUp(dto, ct);

        return CreatedAtAction(
            nameof(SignUp),
            new { id = userId },
            new { id = userId }
        );
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto, CancellationToken ct)
    {
        var token = await authService.Login(dto, ct);

        return Ok(token);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetUserById(Guid id, CancellationToken ct)
    {
        var user = await userService.GetUser(id, ct)
            ?? throw new ServiceException($"User does not exist with ID {id}");

        return Ok(user);
    }
}