using FaceAnalyzer.Api.Data.Entities;
using Swashbuckle.AspNetCore.Annotations;

namespace FaceAnalyzer.Api.Service.Contracts;

[SwaggerSchema(Title = nameof(CreateStimuliDto))]
public record CreateStimuliDto(
    string Link,
    string Name,
    int ExperimentId,
    string Description);