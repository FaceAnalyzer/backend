using Swashbuckle.AspNetCore.Annotations;

namespace FaceAnalyzer.Api.Service.Contracts;

[SwaggerSchema(Title = nameof(LoginRequest))]
public record LoginRequest(
    string Username,
    string Password
);