﻿using FaceAnalyzer.Api.Business.Commands.Stimuli;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Business.Queries;
using FaceAnalyzer.Api.Service.Contracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FaceAnalyzer.Api.Service.Controllers;

[ApiController]
[Route("stimuli")]
public class StimuliController : ControllerBase
{
    private readonly ISender _mediator;

    public StimuliController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<StimuliDto>> Get(int id)
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
    public async Task<ActionResult<IList<StimuliDto>>> Create([FromBody] CreateStimuliDto dto)
    {
        var request = new CreateStimuliCommand(
            dto.Link,
            dto.Description,
            dto.ExperimentId
        );
        var result = await _mediator.Send(request);

        return Created($"stimuli/{result.Id}", result);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _mediator.Send(new DeleteStimuliCommand(id));
        return NoContent();
    }
}