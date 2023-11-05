using FaceAnalyzer.Api.Data.Entities;

namespace FaceAnalyzer.Api.Service.Contracts;

public record CreateReactionDto( int StimuliId, string PartecipantName);