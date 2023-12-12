using FaceAnalyzer.Api.Business.Commands.Stimuli;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Business.Queries;
using FaceAnalyzer.Api.Data.Entities;
using FaceAnalyzer.Api.Service.Contracts;
using FaceAnalyzer.Api.Service.Swagger.Examples;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

namespace FaceAnalyzer.Api.Service.Controllers;

[ApiController]
[Route("stimuli")]
[SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
public class StimuliController : ControllerBase
{
    private readonly ISender _mediator;

    public StimuliController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id:int}")]
    [SwaggerOperation("Retrieve a single stimuli.",
        "Retrieve a single stimuli given its Id.",
        OperationId = $"{nameof(Stimuli)}_get")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(StimuliDto))]
    public async Task<ActionResult<StimuliDto>> GetById(int id)
    {
        var request = new GetStimuliQuery
        {
            Id = id
        };
        var result = await _mediator.Send(request);
        var stimuli = result.Items.FirstOrDefault();
        return Ok(stimuli);
    }

    [HttpGet]
    [SwaggerOperation("Retrieve a list of stimuli.",
        "Retrieve a list of all stimuli that can be filtered by experiment [experimentId].",
        OperationId = $"{nameof(Stimuli)}_get_list")]
    [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(QueryResult<StimuliDto>))]
    public async Task<ActionResult<QueryResult<StimuliDto>>> Get([FromQuery] StimuliQueryDto queryDto)
    {
        var request = new GetStimuliQuery
        {
            ExperimentId = queryDto.ExperimentId
        };
        var result = await _mediator.Send(request);

        return Ok(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [SwaggerOperation("Create a stimuli.",
        "Create a stimuli with a link to a video [link], a [description], a [name] and an [experimentId] to associate it with.",
        OperationId = $"{nameof(Stimuli)}create")]
    [SwaggerResponse(StatusCodes.Status201Created, Type = typeof(List<StimuliDto>))]
    [SwaggerRequestExample(typeof(CreateStimuliDto), typeof(CreateStimuliDtoExample))]
    public async Task<ActionResult<IList<StimuliDto>>> Create([FromBody] CreateStimuliDto dto)
    {
        var request = new CreateStimuliCommand(
            Link: dto.Link,
            Description: dto.Description,
            Name: dto.Name,
            ExperimentId: dto.ExperimentId
        );
        var result = await _mediator.Send(request);

        return CreatedAtAction(nameof(GetById), new
        {
            id= result.Id
        }, result);
    }

    [HttpDelete("{id:int}")]
    [SwaggerOperation("Delete a stimuli.",
        "Delete a single stimuli given its Id.",
        OperationId = $"{nameof(Stimuli)}_delete")]
    [SwaggerResponse(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(int id)
    {
        await _mediator.Send(new DeleteStimuliCommand(id));
        return NoContent();
    }
}