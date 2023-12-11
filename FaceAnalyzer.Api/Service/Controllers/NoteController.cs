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
using Swashbuckle.AspNetCore.Annotations;


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
    [SwaggerOperation("Retrieve a single note",
        "Retrieve a single Note given its Id.",
        OperationId = $"{nameof(NoteController)}_get")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(NoteDto))]
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
    [SwaggerOperation("Retrieve a list of notes",
        "Retrieve a list of notes given filtered by their [experimentId] (if [experimentId is empty all notes are returned).",
        OperationId = $"{nameof(NoteController)}_get_list")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(List<NoteDto>))]
    public async Task<ActionResult<List<NoteDto>>> Get(int? experimentId)
    {
        var result = await _mediator.Send(new GetNotesQuery(null, experimentId));
        return Ok(result);
    }

    [HttpPost]
    [SwaggerOperation("Create a note",
        "Create a single note given the content of the note [description], the [experimentId] to associate it with, and the [creatorId].",
        OperationId = $"{nameof(NoteController)}_create")]
    [SwaggerResponse(StatusCodes.Status201Created, Type = typeof(NoteDto))]
    public async Task<ActionResult<NoteDto>> Create([FromBody] CreateNoteCommand dto)
    {
        var result = await _mediator.Send(dto);
        return CreatedAtAction(nameof(Get), new
        {
            id = result.Id
        }, result);
    }

    [HttpPut("{id}")]
    [SwaggerOperation("Modify a Note",
        "Modify a Note content given its Id. Only the content of the note is modifiable [description] ",
        OperationId = $"{nameof(NoteController)}_modify")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(NoteDto))]
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
    [SwaggerOperation("Delete a Note",
        "Delete a Note given its Id.",
        OperationId = $"{nameof(NoteController)}_delete")]
    [SwaggerResponse(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<NoteDto>> Delete(int id)
    {
        var command = new DeleteNoteCommand(id);
        await _mediator.Send(command);
        return NoContent();
    }
}