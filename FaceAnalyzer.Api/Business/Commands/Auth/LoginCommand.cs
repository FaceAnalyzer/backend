using FaceAnalyzer.Api.Business.Contracts;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;

namespace FaceAnalyzer.Api.Business.Commands.Auth;

[SwaggerSchema(Title = nameof(LoginCommand))]
public record LoginCommand(
    string Username,
    string Password
) : IRequest<AuthResult>;
