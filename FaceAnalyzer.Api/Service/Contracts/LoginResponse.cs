namespace FaceAnalyzer.Api.Service.Contracts;

public record LoginResponse(
    string AccessToken,
    int UserId
);