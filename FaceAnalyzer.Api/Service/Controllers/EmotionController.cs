using FaceAnalyzer.Api.Business.Commands.Emotions;
using FaceAnalyzer.Api.Business.Contracts;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FaceAnalyzer.Api.Service.Controllers;

[ApiController]
[Route("emotions")]
[SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
public class EmotionController: ControllerBase
{

    private readonly ISender _mediator;

    public EmotionController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [SwaggerOperation("Create an emotion entry.",
        "Create an emotion entry, given an [value], a [timeOffset], an [emotionType], and associate it to a reaction [reactionId].",
        OperationId = $"{nameof(EmotionController)}_create")]
    [SwaggerResponse(StatusCodes.Status201Created, Type = typeof(EmotionDto))]
    public async Task<ActionResult<EmotionDto>> CreateEmotion([FromBody] CreateEmotionCommand request)
    {
        var emotionDto = await _mediator.Send(request);
        return Created($"/emotions/{emotionDto.Id}", emotionDto);
    }
}