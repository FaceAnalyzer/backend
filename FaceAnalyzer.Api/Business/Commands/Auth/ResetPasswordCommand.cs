using FaceAnalyzer.Api.Business.Contracts;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;

namespace FaceAnalyzer.Api.Business.Commands.Auth;

[SwaggerSchema(Title = nameof(ResetPasswordCommand))]
public record ResetPasswordCommand(
    int UserId,
    string NewPassword
) : IRequest<ResetPasswordResult>;