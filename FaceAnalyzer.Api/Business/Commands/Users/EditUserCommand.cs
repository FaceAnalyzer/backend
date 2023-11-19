using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Shared.Enum;
using MediatR;

namespace FaceAnalyzer.Api.Business.Commands.Users;

public record EditUserCommand(int Id, string Name, 
    string Surname,
    string Email,
    string Username,
    string ContactNumber,
    UserRole Role): IRequest<UserDto>;