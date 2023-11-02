using FaceAnalyzer.Api.Data.Entities;
using FaceAnalyzer.Api.Shared.Enum;

namespace FaceAnalyzer.Api.Service.Contracts;

public record CreateEmotionDto(int Id, double Value, long TimeOffset, EmotionType EmotionType, int ReactionID, Reaction Reaction);

