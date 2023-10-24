namespace FaceAnalyzer.Api.Data.Entities;

public class Stimuli: EntityBase, IDeletable
{
    public required string Link { get; set; }
    public required int ExperimentId { get; set; }
    
    public DateTime? DeletedAt { get; set; }
    public string? DeletedBy { get; set; }

    public Experiment? Experiment { get; set; }
}