using FaceAnalyzer.Api.Shared.Enum;

namespace FaceAnalyzer.Api.Service.Contracts;

public record CreateUserDto(string Name, string Surname,
    string Email, string Username, string Password, string ContactNumber, UserRole Role);