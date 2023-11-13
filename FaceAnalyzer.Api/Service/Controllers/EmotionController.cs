using FaceAnalyzer.Api.Business.Commands.Emotions;
using FaceAnalyzer.Api.Business.Contracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FaceAnalyzer.Api.Service.Controllers;

[ApiController]
[Route("emotions")]
public class EmotionController: ControllerBase
{

    private readonly ISender _mediator;

    public EmotionController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<ActionResult<EmotionDto>> CreateEmotion([FromBody] CreateEmotionCommand request)
    {
        var emotionDto = await _mediator.Send(request);
        return Created($"/emotions/{emotionDto.Id}", emotionDto);
    }
}