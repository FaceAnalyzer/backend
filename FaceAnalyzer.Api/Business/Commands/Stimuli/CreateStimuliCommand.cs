using FaceAnalyzer.Api.Business.Contracts;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;

namespace FaceAnalyzer.Api.Business.Commands.Stimuli;

[SwaggerSchema(Title = nameof(CreateStimuliCommand))]
public record CreateStimuliCommand
    (
        string Link,
        string Description,
        string Name,
        int ExperimentId)
    : IRequest<StimuliDto>;