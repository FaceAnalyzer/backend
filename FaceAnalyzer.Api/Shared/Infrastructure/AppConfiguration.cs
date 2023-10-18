namespace FaceAnalyzer.Api.Shared.Infrastructure;

public record ConnectionStrings
{
    public string AppDatabase { get; set; }
}

public record AppConfiguration
{
    public ConnectionStrings ConnectionStrings { get; set; }
}