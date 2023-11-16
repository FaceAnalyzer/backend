namespace FaceAnalyzer.Api.Data.Entities;

public class Project : EntityBase, IDeletable
{
    public string Name { get; set; }

    #region Navigations

    public ICollection<Experiment> Experiments { get; set; } = new List<Experiment>();
    public ICollection<User> Users { get; set; } = new List<User>();

    #endregion

    public DateTime? DeletedAt { get; set; }

    public void Delete()
    {
        foreach (var experiment in Experiments)
        {
            experiment.Delete();
        }

        Users.Clear();
        DeletedAt = DateTime.Now;
    }
}