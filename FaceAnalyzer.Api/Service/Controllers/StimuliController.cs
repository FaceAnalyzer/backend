using FaceAnalyzer.Api.Business.Commands.Stimuli;
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

    [HttpGet]
    public async Task<ActionResult<QueryResult<StimuliDto>>> Get()
    {
        var request = new GetStimuliQuery(null);
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

        return Created($"stimuli/{result.Id}",result);
    }
}