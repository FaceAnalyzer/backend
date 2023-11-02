using FaceAnalyzer.Api.Data.Entities;

namespace FaceAnalyzer.Api.Service.Contracts;

public record CreateReactionDto(int Id, int StimuliId, string PartecipantName, Stimuli stimuli, ICollection<Emotion> Emotions);