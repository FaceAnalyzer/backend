using FaceAnalyzer.Api.Business.Contracts;
using MediatR;

namespace FaceAnalyzer.Api.Business.Commands.Auth;

public record ResetPasswordCommand(
    int UserId,
    string NewPassword
) : IRequest<ResetPasswordResult>;