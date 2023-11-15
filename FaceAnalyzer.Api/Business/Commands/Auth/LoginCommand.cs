using FaceAnalyzer.Api.Business.Contracts;
using MediatR;

namespace FaceAnalyzer.Api.Business.Commands.Auth;

public record LoginCommand(
    string Username,
    string Password
) : IRequest<AuthResult>;
