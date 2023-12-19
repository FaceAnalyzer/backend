using System.ComponentModel.DataAnnotations;
using FaceAnalyzer.Api.Shared.Enum;
using Swashbuckle.AspNetCore.Annotations;

namespace FaceAnalyzer.Api.Service.Contracts;

[SwaggerSchema(Title = nameof(EditUserDto))]
public record EditUserDto(
    string Name,
    string Surname,
    string Email,
    string Username,
    string ContactNumber,
    UserRole Role);