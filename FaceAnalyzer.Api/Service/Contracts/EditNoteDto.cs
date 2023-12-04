namespace FaceAnalyzer.Api.Service.Contracts;

public record EditNoteDto(string Description, int? ExperimentId, int? CreatorId);
