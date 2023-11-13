using FaceAnalyzer.Api.Business.BusinessModels;
using FaceAnalyzer.Api.Business.Commands.Experiments;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Business.Queries;
using FaceAnalyzer.Api.Service.Contracts;
using FaceAnalyzer.Api.Shared.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FaceAnalyzer.Api.Service.Controllers;

[ApiController]
[Route("experiments")]
public class ExperimentController : ControllerBase
{
    private readonly ISender _mediator;

    public ExperimentController(ISender mediator)
    {
        ;
        _mediator = mediator;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ExperimentDto>> Get(int id)
    {
        var result = await _mediator.Send(new GetExperimentsQuery(id));
        if (result.Items.Count == 0)
        {
            throw new EntityNotFoundException("Experiment", id);
        }

        return Ok(result.Items.FirstOrDefault());
    }

    [HttpGet]
    public async Task<ActionResult<List<ExperimentDto>>> Get()
    {
        var result = await _mediator.Send(new GetExperimentsQuery(null));
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ExperimentDto>> Create([FromBody] CreateExperimentCommand dto)
    {
        var result = await _mediator.Send(dto);
        return CreatedAtAction(nameof(Get), new
        {
            id = result.Id
        }, result);
    }

    [HttpPut("{id}")]
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
    public async Task<ActionResult<ExperimentDto>> Delete(int id)
    {
        var command = new DeleteExperimentCommand(id);
        await _mediator.Send(command);
        return NoContent();
    }
}