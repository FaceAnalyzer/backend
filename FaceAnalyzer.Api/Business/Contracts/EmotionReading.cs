using FaceAnalyzer.Api.Shared.Enum;
using Swashbuckle.AspNetCore.Annotations;

namespace FaceAnalyzer.Api.Business.Contracts;

[SwaggerSchema(nameof(EmotionReading))]
public record EmotionReading(
    long Time,
    IDictionary<EmotionType, double> Values
);
