using Swashbuckle.AspNetCore.Annotations;

namespace FaceAnalyzer.Api.Service.Contracts;

[SwaggerSchema(Title = nameof(EditExperimentDto))]
public record EditExperimentDto(string Name, string Description, int? ProjectId);