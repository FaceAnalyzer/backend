namespace FaceAnalyzer.Api.Data.Entities;

public class Reaction : EntityBase, IDeletable
{
    public required int StimuliId { get; set; }
    public required string ParticipantName { get; set; }

    public Stimuli Stimuli { get; set; }
    public ICollection<Emotion> Emotions { get; set; }
    public DateTime? DeletedAt { get; set; }
}