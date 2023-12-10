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
    [SwaggerOperation("Create an Emotion entry.",
        "Create an emotion entry, given an emotion type, and a value. The created emotion is associated with a Reaction.",
        OperationId = $"{nameof(EmotionController)}_create")]
    public async Task<ActionResult<EmotionDto>> CreateEmotion([FromBody] CreateEmotionCommand request)
    {
        var emotionDto = await _mediator.Send(request);
        return Created($"/emotions/{emotionDto.Id}", emotionDto);
    }
}