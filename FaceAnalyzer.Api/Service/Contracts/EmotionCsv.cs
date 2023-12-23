using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Shared.Enum;

namespace FaceAnalyzer.Api.Service.Contracts;

public class EmotionCsv
{
    public EmotionCsv(ICollection<EmotionDto> emotionDto)
    {
        
        TimeOffset = emotionDto.First().TimeOffset;
        Anger = emotionDto.First(r => r.EmotionType == EmotionType.Anger).Value;
        Disgust = emotionDto.First(r => r.EmotionType == EmotionType.Disgust).Value;
        Fear = emotionDto.First(r => r.EmotionType == EmotionType.Fear).Value;
        Happiness = emotionDto.First(r => r.EmotionType == EmotionType.Happiness).Value;
        Sadness = emotionDto.First(r => r.EmotionType == EmotionType.Sadness).Value;
        Surprise = emotionDto.First(r => r.EmotionType == EmotionType.Surprise).Value;
        Neutral = emotionDto.First(r => r.EmotionType == EmotionType.Neutral).Value;
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