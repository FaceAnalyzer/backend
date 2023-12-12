using FaceAnalyzer.Api.Business.Commands.Emotions;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Service.Contracts;
using FaceAnalyzer.Api.Service.Swagger.Examples;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Filters;

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
    [SwaggerRequestExample(typeof(CreateEmotionDto), typeof(CreateEmotionDtoExample))]
    public async Task<ActionResult<EmotionDto>> CreateEmotion([FromBody] CreateEmotionDto request)
    {
        var command =
            new CreateEmotionCommand(request.Value, request.TimeOffset, request.EmotionType, request.ReactionId);
        var emotionDto = await _mediator.Send(command);
        return Created($"/emotions/{emotionDto.Id}", emotionDto);
    }
}