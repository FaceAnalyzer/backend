namespace FaceAnalyzer.Api.Service.Contracts;

public record CreateExperimentDto(int Id, string Name, string Description, int ProjectId);