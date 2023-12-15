using Swashbuckle.AspNetCore.Annotations;

namespace FaceAnalyzer.Api.Service.Contracts;

[SwaggerSchema(Title = nameof(GrantRevokeProjectPermissionDto))]
public record GrantRevokeProjectPermissionDto(IList<int> ResearchersIds);