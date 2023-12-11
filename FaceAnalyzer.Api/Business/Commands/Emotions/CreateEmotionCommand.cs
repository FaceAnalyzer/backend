using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Shared.Enum;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;

namespace FaceAnalyzer.Api.Business.Commands.Emotions;

[SwaggerSchema(Title = nameof(CreateEmotionCommand))]
public record CreateEmotionCommand
    (double Value, long TimeOffset, EmotionType EmotionType, int ReactionId) : IRequest<EmotionDto>;