using FaceAnalyzer.Api.Data.Entities;
using Swashbuckle.AspNetCore.Annotations;

namespace FaceAnalyzer.Api.Business.Contracts;

[SwaggerSchema(Title = nameof(StimuliDto))]
public record StimuliDto(
    int Id,
    string Link,
    string Name,
    int ExperimentId,
    string Description);

