using System.Globalization;
using CsvHelper;
using FaceAnalyzer.Api.Business.Commands.Reactions;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Business.Queries;
using FaceAnalyzer.Api.Data.Entities;
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

    [HttpGet("{id}/emotions")]
    public async Task<ActionResult<QueryResult<EmotionDto>>> GetReactionEmotions(int id)
    {
        var query = new GetReactionEmotionsQuery(id);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}/emotions/export")]
    public async Task<IActionResult> GetReactionEmotionsCsv(int id)
    {
        var query = new GetReactionEmotionsQuery(id);
        var result = await _mediator.Send(query);
        
        // Convert list to a csv file
        var stream = new MemoryStream();
        await using (var writeFile = new StreamWriter(stream, leaveOpen: true))
        await using(var csv = new CsvWriter(writeFile, CultureInfo.InvariantCulture))
        {
            await csv.WriteRecordsAsync(result.Items);
        }
        stream.Position = 0;
        return new FileStreamResult(stream, "text/csv"){ FileDownloadName = $"{nameof(Reaction)}#{id}.csv"};
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