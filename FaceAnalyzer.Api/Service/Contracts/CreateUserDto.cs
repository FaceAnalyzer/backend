using System.ComponentModel.DataAnnotations;
using FaceAnalyzer.Api.Shared.Enum;

namespace FaceAnalyzer.Api.Service.Contracts;

public record CreateUserDto(
    string Name,
    string Surname,
    [EmailAddress] string Email,
    [Required] string Username,
    [Required] string Password,
    [Phone] string ContactNumber,
    [EnumDataType(typeof(UserRole))] UserRole Role);