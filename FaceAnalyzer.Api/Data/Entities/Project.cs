namespace FaceAnalyzer.Api.Data.Entities;

public class Project : EntityBase, IDeletable
{
    public string Name { get; set; }

    #region Navigations

    public ICollection<Experiment> Experiments { get; set; }
    public ICollection<User> Users { get; set; }

    #endregion

    public DateTime? DeletedAt { get; set; }
}