using FaceAnalyzer.Api.Shared.Enum;
using Swashbuckle.AspNetCore.Annotations;

namespace FaceAnalyzer.Api.Service.Contracts;

[SwaggerSchema(Title = nameof(UserQueryDto))]
public record class UserQueryDto(int? ProjectId, UserRole? Role);