using FaceAnalyzer.Api.Shared.Enum;

namespace FaceAnalyzer.Api.Service.Contracts;

public record EditUserDto(string Name, 
    string Surname,
    string Email,
    string Username,
    string ContactNumber,
    UserRole Role);