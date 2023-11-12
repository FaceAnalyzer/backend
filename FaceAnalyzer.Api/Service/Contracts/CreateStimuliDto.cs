using FaceAnalyzer.Api.Data.Entities;

namespace FaceAnalyzer.Api.Service.Contracts;

public record CreateStimuliDto(
    string Link,
    string Name,
    int ExperimentId,
    string Description);