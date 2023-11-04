using FaceAnalyzer.Api.Business.BusinessModels;
using FaceAnalyzer.Api.Business.Commands.Experiments;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Business.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FaceAnalyzer.Api.Service.Controllers;

[ApiController]
[Route("experiments")]
public class ExperimentController : ControllerBase
{
    private readonly ISender _mediator;

    public ExperimentController(ISender mediator)
    {;
        _mediator = mediator;
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
        return Created($"/experiments/{result.Id}", result);
    }

    [HttpPut]
    public async Task<ActionResult<ExperimentDto>> Edit([FromBody] EditExperimentCommand dto)
    {
        var result = await _mediator.Send(dto);
        return Ok(result);
    }
    
    [HttpDelete]
    public async Task<ActionResult<ExperimentDto>> Delete([FromBody] DeleteExperimentCommand dto)
    {
        var result = await _mediator.Send(dto);
        return NoContent();
    }
}