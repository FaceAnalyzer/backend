namespace FaceAnalyzer.Api.Business.Contracts;

public record ExperimentDto
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required int ProjectId { get; set; }
}