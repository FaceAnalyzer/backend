namespace FaceAnalyzer.Api.Shared;

public record ConnectionStrings
{
    public string AppDatabase { get; set; }
}

public record AppConfiguration
{
    public ConnectionStrings ConnectionStrings { get; set; }
}