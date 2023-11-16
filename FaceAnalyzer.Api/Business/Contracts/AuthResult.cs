namespace FaceAnalyzer.Api.Business.Contracts;

public record AuthResult(
    string AccessToken,
    int Id
);