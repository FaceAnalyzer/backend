using FaceAnalyzer.Api.Business.Contracts;

namespace FaceAnalyzer.Api.Service.Contracts;


public record CreateReactionDto(
    int StimuliId,
    string ParticipantName,
    IList<EmotionReading> EmotionReadings);