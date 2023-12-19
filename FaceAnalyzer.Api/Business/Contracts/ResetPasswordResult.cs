using FaceAnalyzer.Api.Business.Commands.Auth;
using Swashbuckle.AspNetCore.Annotations;

namespace FaceAnalyzer.Api.Business.Contracts;

[SwaggerSchema(Title = nameof(ResetPasswordCommand))]
public record ResetPasswordResult(int UserId);