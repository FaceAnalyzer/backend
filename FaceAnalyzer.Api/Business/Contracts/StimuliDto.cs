using FaceAnalyzer.Api.Data.Entities;

namespace FaceAnalyzer.Api.Business.Contracts;

public record StimuliDto(string Link, int ExperimentId, string Description, Experiment Experiment, ICollection<Reaction> Reactions);