using FaceAnalyzer.Api.Business.Commands.Reactions;
using FaceAnalyzer.Api.Business.Contracts;
using Swashbuckle.AspNetCore.Annotations;

namespace FaceAnalyzer.Api.Service.Contracts;


[SwaggerSchema(Title = nameof(CreateReactionDto))]
public record CreateReactionDto(
    int StimuliId,
    string ParticipantName,
    IList<EmotionReading> EmotionReadings);