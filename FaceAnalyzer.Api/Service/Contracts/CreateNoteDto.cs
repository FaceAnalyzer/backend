namespace FaceAnalyzer.Api.Service.Contracts;

public record CreateNoteDto(string Description, int ExperimentId, int CreatorId);