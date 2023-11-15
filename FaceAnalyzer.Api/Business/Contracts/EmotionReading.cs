using FaceAnalyzer.Api.Shared.Enum;

namespace FaceAnalyzer.Api.Business.Contracts;

public record EmotionReading(
    long Time,
    IDictionary<EmotionType, double> Values
);
