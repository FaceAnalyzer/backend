using FaceAnalyzer.Api.Business.Commands.Reactions;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Business.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;




namespace FaceAnalyzer.Api.Service.Controllers;

[ApiController]
[Route("reactions")]

public class ReactionController : ControllerBase
{
    private readonly ISender _mediator;
    
    public ReactionController(ISender mediator)
    {;
        _mediator = mediator;
    }
    
    [HttpGet]
    public async Task<ActionResult<List<ReactionDto>>> Get()
    {
        var result = await _mediator.Send(new GetReactionsQuery(null));
        return Ok(result);
    }
    
    [HttpPost]
    public async Task<ActionResult<ReactionDto>> Create([FromBody] CreateReactionCommand dto)
    {
        var result = await _mediator.Send(dto);
        return Created($"/reactions/{result.Id}", result);
    }
    
    [HttpDelete]
    public async Task<ActionResult<ReactionDto>> Delete([FromBody] DeleteReactionCommand dto)
    {
        var result = await _mediator.Send(dto);
        return NoContent();
    }
    
    
    
    
    
    
}