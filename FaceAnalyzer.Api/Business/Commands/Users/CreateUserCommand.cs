using FaceAnalyzer.Api.Shared.Enum;
using FaceAnalyzer.Api.Business.Contracts;
using MediatR;

namespace FaceAnalyzer.Api.Business.Commands.Users;

public record CreateUserCommand(
    string Name, 
    string Surname,
    string Email,
    string Username,
    string Password,
    string? ContactNumber,
    UserRole Role): IRequest<UserDto>;