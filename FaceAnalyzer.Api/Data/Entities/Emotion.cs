using FaceAnalyzer.Api.Shared.Enum;
using Swashbuckle.AspNetCore.Annotations;

namespace FaceAnalyzer.Api.Data.Entities;

[SwaggerSchema(Title = "Emotion")]
public class Emotion : EntityBase, IDeletable
{
    public double Value { get; set; }
    public long TimeOffset { get; set; }
    public EmotionType EmotionType { get; set; }
    public int ReactionId { get; set; }
    public Reaction Reaction { get; set; }
    public DateTime? DeletedAt { get; set; }

    public void Delete()
    {
        DeletedAt = DateTime.Now;
    }
}