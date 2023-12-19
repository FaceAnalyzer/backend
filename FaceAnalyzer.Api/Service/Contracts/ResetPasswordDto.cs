using Swashbuckle.AspNetCore.Annotations;

namespace FaceAnalyzer.Api.Service.Contracts;

[SwaggerSchema(Title = nameof(ResetMyPasswordDto))]
public record ResetMyPasswordDto(string NewPassword);

[SwaggerSchema(Title = nameof(ResetUserPasswordDto))]
public record ResetUserPasswordDto(int UserId, string NewPassword);