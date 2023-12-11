using System.Collections.ObjectModel;
using Swashbuckle.AspNetCore.Annotations;

namespace FaceAnalyzer.Api.Data.Entities;

[SwaggerSchema(Title = "Experiment")]
public class Experiment : EntityBase, IDeletable
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required int ProjectId { get; set; }

    #region Navigations

    public Project Project { get; set; }
    public ICollection<Stimuli> Stimuli { get; set; } = new List<Stimuli>();

    #endregion

    public DateTime? DeletedAt { get; set; }

    public void Delete()
    {
        foreach (var stimulus in Stimuli)
        {
            stimulus.Delete();
        }

        DeletedAt = DateTime.Now;
    }
}