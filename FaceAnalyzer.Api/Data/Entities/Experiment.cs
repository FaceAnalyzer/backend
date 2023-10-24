using System.Collections.ObjectModel;

namespace FaceAnalyzer.Api.Data.Entities;

public class Experiment : EntityBase, IDeletable
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required int ProjectId { get; set; }

    #region Navigations

    public Project? Project { get; set; }
    public ICollection<Stimuli> Stimuli { get; set; }
    #endregion
    
    public DateTime? DeletedAt { get; set; }
    public string? DeletedBy { get; set; }
}