using FaceAnalyzer.Api.Data.Entities;

namespace FaceAnalyzer.Api.Business.Contracts;

public record ReactionDto(int Id, int StimuliId, string ParticipantName);