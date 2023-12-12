using Swashbuckle.AspNetCore.Annotations;

namespace FaceAnalyzer.Api.Service.Contracts;

[SwaggerSchema(Title = nameof(LoginResponse))]
public record LoginResponse(
    string AccessToken,
    int UserId
);