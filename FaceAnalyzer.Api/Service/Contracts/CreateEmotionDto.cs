using FaceAnalyzer.Api.Data.Entities;
using FaceAnalyzer.Api.Shared.Enum;

namespace FaceAnalyzer.Api.Service.Contracts;

public record CreateEmotionDto( double Value, long TimeOffset, EmotionType EmotionType);

