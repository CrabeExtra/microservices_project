

using Audit.Application.Dtos;
using Audit.Application.Service.Interface;
using Audit.Domain.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Audit.Api.Controller;

[ApiController]
[Route("api/record")]
public class RecordController(
    IRecordService recordService
) : ControllerBase
{

    [HttpPost]
    public async Task<IActionResult> CreateRecord([FromBody] CreateRecordDto record, CancellationToken cancellationToken)
    {

        // could use a mapper here.
        var domRecord = new Record
        {
            Id = Guid.NewGuid(),
            OldData = record.OldData,
            NewData = record.NewData,
            MicroserviceName = record.MicroserviceName,
            EntityName = record.EntityName,
            Action = record.Action,
            EventType = record.EventType,
            ReferenceId = record.ReferenceId
        };

        var guid = await recordService.CreateRecord(domRecord, cancellationToken);
        return Ok(guid);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetRecordById(Guid id, CancellationToken cancellationToken)
    {
        var record = await recordService.GetById(id, cancellationToken);
        if (record == null)
        {
            return NotFound();
        }
        return Ok(record);
    }

    [HttpGet("field")]
    public async Task<IActionResult> GetRecordByField(
        [FromQuery] string fieldName,
        [FromQuery] string value,
        CancellationToken cancellationToken
    )
    {
        var records = await recordService.GetByField(fieldName, value, cancellationToken);

        return Ok(records);
    }
}