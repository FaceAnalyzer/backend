using FaceAnalyzer.Api.Data.Entities;

namespace FaceAnalyzer.Api.Business.Contracts;

public record StimuliDto(int Id, string Link, int ExperimentId, string Description);