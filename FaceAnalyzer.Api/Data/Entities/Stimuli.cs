using Swashbuckle.AspNetCore.Annotations;

namespace FaceAnalyzer.Api.Data.Entities;

[SwaggerSchema(Title = "Stimuli")]
public class Stimuli : EntityBase, IDeletable
{
    public required string Link { get; set; }
    public required int ExperimentId { get; set; }
    public required string Description { get; set; }
    public string Name { get; set; }


    public Experiment Experiment { get; set; }
    public ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();
    public DateTime? DeletedAt { get; set; }

    public void Delete()
    {
        foreach (var reaction in Reactions)
        {
            reaction.Delete();
        }
        DeletedAt = DateTime.Now;
    }
}