using FaceAnalyzer.Api.Shared.Enum;

namespace FaceAnalyzer.Api.Business.Contracts;

public record UserDto(int Id, string Name, string Surname,
    string Email, string Username, string ContactNumber, UserRole Role);