using FaceAnalyzer.Api.Shared.Enum;
using Swashbuckle.AspNetCore.Annotations;

namespace FaceAnalyzer.Api.Business.Contracts;

[SwaggerSchema(Title = nameof(UserDto))]
public record UserDto(int Id, string Name, string Surname,
    string Email, string Username, string ContactNumber, UserRole Role);