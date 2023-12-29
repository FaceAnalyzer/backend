namespace FaceAnalyzer.Api.Business.Contracts;

public record ExportReactionDto(int Id, string ParticipantName, ICollection<EmotionDto> Emotions);