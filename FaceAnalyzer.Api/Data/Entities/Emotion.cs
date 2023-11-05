using FaceAnalyzer.Api.Shared.Enum;

namespace FaceAnalyzer.Api.Data.Entities;

public class Emotion : EntityBase, IDeletable
{
    public double Value { get; set; }
    public long TimeOffset { get; set; }
    public EmotionType EmotionType { get; set; }
    public int ReactionId { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string? DeletedBy { get; set; }

    public Reaction Reaction { get; set; }
}