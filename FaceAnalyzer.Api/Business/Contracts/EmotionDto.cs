using FaceAnalyzer.Api.Data.Entities;
using FaceAnalyzer.Api.Shared.Enum;

namespace FaceAnalyzer.Api.Business.Contracts;

public record EmotionDto(int Id, double Value, long TimeOffset, EmotionType EmotionType, int ReactionID, Reaction Reaction);
