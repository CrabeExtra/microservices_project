
using Personal.Application.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Personal.Domain.Entity;
using System.Security.Cryptography;
using System.Text;

namespace Personal.Api.Controller;

[ApiController]
public class SkillsController(
    ISkillService skillsService,
    IConfiguration configuration
) : BaseController
{

    private readonly string adminKey = configuration["ADMIN_KEY"]
            ?? throw new InvalidOperationException("ADMIN_KEY is not configured.");
    private bool IsAdmin(string? key)
    {
        if (string.IsNullOrEmpty(key)) return false;

        return CryptographicOperations.FixedTimeEquals(
            Encoding.UTF8.GetBytes(key),
            Encoding.UTF8.GetBytes(adminKey)
        );
    }

    [HttpGet]
    public async Task<ActionResult<PagedData<Skill>>> GetSkills(
        [FromQuery] string search = "",
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string sortField = "Name",
        [FromQuery] bool asc = true,
        CancellationToken ct = default)
    {
        var result = await skillsService.GetSkills(
            search, page, pageSize, sortField, asc, ct);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddSkill(
        [FromBody] AdminRequest<Skill> request,
        CancellationToken ct)
    {
        if (!IsAdmin(request.Key))
            return Unauthorized();

        await skillsService.AddSkill(request.Data, ct);

        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> RemoveSkill(
        [FromBody] AdminRequest<string> request,
        CancellationToken ct)
    {
        if (!IsAdmin(request.Key))
            return Unauthorized();

        await skillsService.RemoveSkill(request.Data, ct);

        return NoContent();
    }
}