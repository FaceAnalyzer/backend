using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Shared.Enum;

namespace FaceAnalyzer.Api.Service.Contracts;

public class EmotionCsv
{
    public EmotionCsv(ICollection<EmotionDto> emotionDto)
    {
        
        TimeOffset = emotionDto.First().TimeOffset;
        Anger = emotionDto.FirstOrDefault(r => r.EmotionType == EmotionType.Anger)?.Value ?? 0.0;
        Disgust = emotionDto.FirstOrDefault(r => r.EmotionType == EmotionType.Disgust)?.Value ?? 0.0;
        Fear = emotionDto.FirstOrDefault(r => r.EmotionType == EmotionType.Fear)?.Value ?? 0.0;
        Happiness = emotionDto.FirstOrDefault(r => r.EmotionType == EmotionType.Happiness)?.Value ?? 0.0;
        Sadness = emotionDto.FirstOrDefault(r => r.EmotionType == EmotionType.Sadness)?.Value ?? 0.0;
        Surprise = emotionDto.FirstOrDefault(r => r.EmotionType == EmotionType.Surprise)?.Value ?? 0.0;
        Neutral = emotionDto.FirstOrDefault(r => r.EmotionType == EmotionType.Neutral)?.Value ?? 0.0;
    }

    public EmotionCsv()
    {
    }

    public long TimeOffset { get; init; }
    public double Anger { get; init; }
    public double Disgust { get; init; }
    public double Fear { get; init; }
    public double Happiness { get; init; }
    public double Sadness { get; init; }
    public double Surprise { get; init; }
    public double Neutral { get; init; }
    
}