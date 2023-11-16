namespace FaceAnalyzer.Api.Data.Entities;

public class Reaction : EntityBase, IDeletable
{
    public required int StimuliId { get; set; }
    public required string ParticipantName { get; set; }

    public Stimuli Stimuli { get; set; }
    public ICollection<Emotion> Emotions { get; set; } = new List<Emotion>();
    public DateTime? DeletedAt { get; set; }

    public void Delete()
    {
        foreach (var emotion in Emotions)
        {
            emotion.Delete();
        }

        DeletedAt = DateTime.Now;
    }
}