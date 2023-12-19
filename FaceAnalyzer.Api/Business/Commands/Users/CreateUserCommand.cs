using FaceAnalyzer.Api.Shared.Enum;
using FaceAnalyzer.Api.Business.Contracts;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;

namespace FaceAnalyzer.Api.Business.Commands.Users;

[SwaggerSchema(Title = nameof(CreateUserCommand))]
public record CreateUserCommand(
    string Name, 
    string Surname,
    string Email,
    string Username,
    string Password,
    string? ContactNumber,
    UserRole Role): IRequest<UserDto>;