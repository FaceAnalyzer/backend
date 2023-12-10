using FaceAnalyzer.Api.Business.BusinessModels;
using FaceAnalyzer.Api.Business.Commands.Notes;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Business.Queries;
using FaceAnalyzer.Api.Data.Entities;
using FaceAnalyzer.Api.Service.Contracts;
using FaceAnalyzer.Api.Shared.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace FaceAnalyzer.Api.Service.Controllers;

[ApiController]
[Route("notes")]
public class NoteController : ControllerBase
{
    private readonly ISender _mediator;
    
    public NoteController(ISender mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<NoteDto>> Get(int id)
    {
        var result = await _mediator.Send(new GetNotesQuery(id, null));
        if (result.Items.Count == 0)
        {
            throw new EntityNotFoundException(nameof(Note), id);
        }

        return Ok(result.Items.FirstOrDefault());
    }
    
    [HttpGet]
    public async Task<ActionResult<List<NoteDto>>> Get(int? experimentId)
    {
        var result = await _mediator.Send(new GetNotesQuery(null, experimentId));
        return Ok(result);
    }
    
    [HttpPost]
    public async Task<ActionResult<NoteDto>> Create([FromBody] CreateNoteCommand dto)
    {
        var result = await _mediator.Send(dto);
        return CreatedAtAction(nameof(Get), new
        {
            id = result.Id
        }, result);
    }
    
    [HttpPut("{id}")]
    public async Task<ActionResult<NoteDto>> Edit(int id, [FromBody] EditNoteDto dto)
    {
        var command = new EditNoteCommand(
            id,
            dto.Description,
            dto.ExperimentId,
            dto.CreatorId
        );
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<NoteDto>> Delete(int id)
    {
        var command = new DeleteNoteCommand(id);
        await _mediator.Send(command);
        return NoContent();
    }
    
}