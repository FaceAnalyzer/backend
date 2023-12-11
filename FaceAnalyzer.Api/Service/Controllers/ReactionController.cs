using System.Globalization;
using CsvHelper;
using FaceAnalyzer.Api.Business.Commands.Reactions;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Business.Queries;
using FaceAnalyzer.Api.Data.Entities;
using FaceAnalyzer.Api.Service.Contracts;
using FaceAnalyzer.Api.Service.Swagger.Examples;
using FaceAnalyzer.Api.Shared.Enum;
using FaceAnalyzer.Api.Shared.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;
using SwaggerExample = Swashbuckle.AspNetCore.Filters.SwaggerExample;


namespace FaceAnalyzer.Api.Service.Controllers;

[ApiController]
[Route("reactions")]
public class ReactionController : ControllerBase
{
    private readonly ISender _mediator;

    public ReactionController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [SwaggerOperation("Retrieve a list of reactions.",
        "Retrieve a list of all reactions.",
        OperationId = $"{nameof(Reaction)}_get_list")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(List<ReactionDto>))]
    public async Task<ActionResult<List<ReactionDto>>> Get()
    {
        var result = await _mediator.Send(new GetReactionsQuery(null));
        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [SwaggerOperation("Retrieve a single reaction.",
        "Retrieve a single reaction given its Id.",
        OperationId = $"{nameof(Reaction)}_get")]
    [SwaggerResponse(StatusCodes.Status200OK, Type=typeof(ReactionDto))]
    public async Task<ActionResult<ReactionDto>> Get(int id)
    {
        var result = await _mediator.Send(new GetReactionsQuery(id));
        if (result.Items.Count == 0)
        {
            throw new EntityNotFoundException("Reaction", id);
        }

        return Ok(result.Items.FirstOrDefault());
    }

    [HttpGet("{id}/emotions")]
    [SwaggerOperation("Retrieve a reaction emotions.",
        "Retrieve a reaction (using its Id) emotions.",
        OperationId = $"{nameof(Reaction)}_get_emotions")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(QueryResult<EmotionDto>))]
    public async Task<ActionResult<QueryResult<EmotionDto>>> GetReactionEmotions(int id, [FromQuery] EmotionType? type)
    {
        var query = new GetReactionEmotionsQuery(id, type);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}/emotions/export")]
    [SwaggerOperation("Export reaction emotions.",
        "Export a reaction (given its Id) emotions as a CSV file.",
        OperationId = $"{nameof(Reaction)}_export")]
    [SwaggerResponse(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetReactionEmotionsCsv(int id)
    {
        var query = new GetReactionEmotionsQuery(id, null);
        var result = await _mediator.Send(query);

        // Convert list to a csv file
        var stream = new MemoryStream();
        await using (var writeFile = new StreamWriter(stream, leaveOpen: true))
        await using (var csv = new CsvWriter(writeFile, CultureInfo.InvariantCulture))
        {
            await csv.WriteRecordsAsync(result.Items);
        }

        stream.Position = 0;
        return new FileStreamResult(stream, "text/csv") { FileDownloadName = $"{nameof(Reaction)}#{id}.csv" };
    }

    [HttpPost]
    [SwaggerRequestExample(typeof(CreateReactionDto), typeof(CreateReactionDtoExample))]
    [SwaggerOperation("Create a reaction.",
        "Create a reaction associated to a stimuli [stimuliId] with a participant [participantName] and readings [emotionReadings].",
        OperationId = $"{nameof(Reaction)}_create")]
    [SwaggerResponse(StatusCodes.Status201Created, Type = typeof(ReactionDto))]
    public async Task<ActionResult<ReactionDto>> Create([FromBody] CreateReactionDto dto)
    {
        var command = new CreateReactionCommand(
            StimuliId: dto.StimuliId,
            ParticipantName: dto.ParticipantName,
            EmotionReadings: dto.EmotionReadings
        );
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(Get), new
        {
            id = result.Id
        }, result);
    }

    [HttpDelete("{id}")]
    [SwaggerOperation("Delete a reaction.",
        "Delete a reaction given its Id.",
        OperationId = $"{nameof(Reaction)}_delete")]
    [SwaggerResponse(StatusCodes.Status204NoContent)]
    public async Task<ActionResult<ReactionDto>> Delete(int id)
    {
        var command = new DeleteReactionCommand(id);
        await _mediator.Send(command);
        return NoContent();
    }
}