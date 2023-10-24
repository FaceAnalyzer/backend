namespace FaceAnalyzer.Api.Business.Contracts;

public record ProjectDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
}