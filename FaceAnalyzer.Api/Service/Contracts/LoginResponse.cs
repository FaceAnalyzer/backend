using FaceAnalyzer.Api.Business.Contracts;
using Swashbuckle.AspNetCore.Annotations;

namespace FaceAnalyzer.Api.Service.Contracts;

[SwaggerSchema(Title = nameof(LoginResponse))]
public record LoginResponse(
    string AccessToken,
    UserDto User
);