using FaceAnalyzer.Api.Business.Commands.Experiments;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Business.Queries;
using FaceAnalyzer.Api.Data.Entities;
using FaceAnalyzer.Api.Service.Contracts;
using FaceAnalyzer.Api.Shared.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FaceAnalyzer.Api.Service.Controllers;

[ApiController]
[Route("experiments")]
[SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
public class ExperimentController : ControllerBase
{
    private readonly ISender _mediator;

    public ExperimentController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id:int}")]
    [SwaggerOperation("Retrieve a single Experiment",
        "Retrieve a single experiment given its Id.",
        OperationId = $"{nameof(Experiment)}_get")]
    public async Task<ActionResult<ExperimentDto>> Get(int id)
    {
        var result = await _mediator.Send(new GetExperimentsQuery(id, null));
        if (result.Items.Count == 0)
        {
            throw new EntityNotFoundException(nameof(Experiment), id);
        }

        return Ok(result.Items.FirstOrDefault());
    }

    [HttpGet]
    [SwaggerOperation("Retrieve a list of Experiments",
        "Retrieve a list of experiments filtered by their [projectId] (if [projectId] is empty all experiments are returned.)",
        OperationId = $"{nameof(Experiment)}_get_list")]
    public async Task<ActionResult<List<ExperimentDto>>> Get(int? projectId)
    {
        var result = await _mediator.Send(new GetExperimentsQuery(null, projectId));
        return Ok(result);
    }

    [HttpPost]
    [SwaggerOperation("Create an Experiment",
        "Create an experiment given its [Name]. The created experiment is associated with a project (with the provided [projectId]).",
        OperationId = $"{nameof(Experiment)}_create")]
    public async Task<ActionResult<ExperimentDto>> Create([FromBody] CreateExperimentCommand dto)
    {
        var result = await _mediator.Send(dto);
        return CreatedAtAction(nameof(Get), new
        {
            id = result.Id
        }, result);
    }

    [HttpPut("{id}")]
    [SwaggerOperation("Modify an Experiment",
        "Modify an Experiment (the only modifiable value is the [Name]).",
        OperationId = $"{nameof(Experiment)}_edit")]
    public async Task<ActionResult<ExperimentDto>> Edit(int id, [FromBody] EditExperimentDto dto)
    {
        var command = new EditExperimentCommand(
            id,
            dto.Name,
            dto.Description,
            dto.ProjectId
        );
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    [SwaggerOperation("Delete an Experiment",
        "Delete an experiment given its Id.",
        OperationId = $"{nameof(Experiment)}_delete")]
    public async Task<ActionResult<ExperimentDto>> Delete(int id)
    {
        var command = new DeleteExperimentCommand(id);
        await _mediator.Send(command);
        return NoContent();
    }
}