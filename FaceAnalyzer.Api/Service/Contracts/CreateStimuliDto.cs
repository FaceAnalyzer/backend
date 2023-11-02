using FaceAnalyzer.Api.Data.Entities;

namespace FaceAnalyzer.Api.Service.Contracts;

public record CreateStimuliDto(string Link, int ExperimentId, string Description, Experiment Experiment, ICollection<Reaction> Reactions);