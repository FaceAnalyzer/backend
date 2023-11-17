namespace FaceAnalyzer.Api.Service.Contracts;

public record LoginRequest(
    string Username,
    string Password
);