namespace FaceAnalyzer.Api.Shared;

public record ConnectionStrings
{
    public string AppDatabase { get; set; }
}


public record JwtConfig
{
    public int Expiry { get; set; } // In Minutes
    public string Secret { get; set; }
}
public record AppConfiguration
{
    public ConnectionStrings ConnectionStrings { get; set; }
    public JwtConfig JwtConfig { get; set; }
    
}