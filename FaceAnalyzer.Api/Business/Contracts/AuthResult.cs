using Swashbuckle.AspNetCore.Annotations;

namespace FaceAnalyzer.Api.Business.Contracts;

[SwaggerSchema(Title = nameof(AuthResult))]
public record AuthResult(
    string AccessToken,
    int Id
);