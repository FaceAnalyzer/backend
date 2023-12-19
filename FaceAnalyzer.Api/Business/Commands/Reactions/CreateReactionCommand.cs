using FaceAnalyzer.Api.Business.Contracts;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;

namespace FaceAnalyzer.Api.Business.Commands.Reactions;

[SwaggerSchema(Title = nameof(CreateReactionCommand))]
public record CreateReactionCommand(
    int StimuliId,
    string ParticipantName,
    IList<EmotionReading> EmotionReadings
    ): IRequest<ReactionDto>;