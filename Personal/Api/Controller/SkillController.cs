
using Personal.Application.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Personal.Domain.Entity;
using System.Security.Cryptography;
using System.Text;
using Personal.Api.Dtos;

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
        [FromQuery] bool asc = false,
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

    [HttpPost("accomplishment")]
    public async Task<IActionResult> AddAccomplishment(
        [FromBody] AdminRequest<AddAccomplishmentDto> request,
        CancellationToken ct)
    {
        if (!IsAdmin(request.Key))
            return Unauthorized();

        await skillsService.AddAccomplishment(request.Data.Text, request.Data.SkillNames, ct);

        return NoContent();
    }

    [HttpPost("synonym")]
    public async Task<IActionResult> AddSynonym(
        [FromBody] AdminRequest<AddSynonymDto> request,
        CancellationToken ct)
    {
        if (!IsAdmin(request.Key))
            return Unauthorized();

        await skillsService.AddSynonym(request.Data.Name, request.Data.SkillName, ct);

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