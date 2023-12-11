using Swashbuckle.AspNetCore.Annotations;

namespace FaceAnalyzer.Api.Service.Contracts;

[SwaggerSchema(Title = nameof(ProjectQueryDto))]
public record ProjectQueryDto(string? ProjectName);