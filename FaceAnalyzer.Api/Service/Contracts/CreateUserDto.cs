using System.ComponentModel.DataAnnotations;
using FaceAnalyzer.Api.Shared.Enum;
using Swashbuckle.AspNetCore.Annotations;

namespace FaceAnalyzer.Api.Service.Contracts;

[SwaggerSchema(Title = nameof(CreateUserDto))]
public record CreateUserDto(
    string Name,
    string Surname,
    [EmailAddress] string Email,
    [Required] string Username,
    [Required] string Password,
    [Phone] string ContactNumber,
    [EnumDataType(typeof(UserRole))] UserRole Role);