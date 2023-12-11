using Swashbuckle.AspNetCore.Annotations;

namespace FaceAnalyzer.Api.Service.Contracts;

[SwaggerSchema(Title = nameof(StimuliQueryDto))]
public class StimuliQueryDto
{
    public int? ExperimentId { get; init; }
};