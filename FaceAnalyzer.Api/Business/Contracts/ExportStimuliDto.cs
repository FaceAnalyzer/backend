namespace FaceAnalyzer.Api.Business.Contracts;

public record ExportStimuliDto(int Id, string Name, ICollection<ExportReactionDto> Reactions);