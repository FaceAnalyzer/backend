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
[SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
public class NoteController : ControllerBase
{
    private readonly ISender _mediator;

    public NoteController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id:int}")]
    [SwaggerOperation("Retrieve a single Note",
        "Retrieve a single Note given its Id.",
        OperationId = $"{nameof(Note)}_get")]
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
    [SwaggerOperation("Retrieve a list of Notes",
        "Retrieve a list of Notes given filtered by their [experimentId] (if [experimentId is empty all notes are returned).",
        OperationId = $"{nameof(Note)}_get_list")]
    public async Task<ActionResult<List<NoteDto>>> Get(int? experimentId)
    {
        var result = await _mediator.Send(new GetNotesQuery(null, experimentId));
        return Ok(result);
    }

    [HttpPost]
    [SwaggerOperation("Create a Note",
        "Create a single Note given the content of the note [description], the [experimentId] to associate it with, and the [creatorId].",
        OperationId = $"{nameof(Note)}_create")]
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
        OperationId = $"{nameof(Note)}_modify")]
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
        OperationId = $"{nameof(Note)}_delete")]
    public async Task<ActionResult<NoteDto>> Delete(int id)
    {
        var command = new DeleteNoteCommand(id);
        await _mediator.Send(command);
        return NoContent();
    }
}