using Swashbuckle.AspNetCore.Annotations;

namespace FaceAnalyzer.Api.Shared.Enum;

[SwaggerSchema(Title = nameof(EmotionType))]
public enum EmotionType
{
    Anger = 10,
    Happiness = 20,
    Disgust = 30,
    Fear = 40,
    Sadness = 50,
    Surprise = 60,
    Neutral = 70,
}