namespace FaceAnalyzer.Api.Data.Entities;

public class Project : EntityBase
{
    public string Name { get; set; }

    #region Navigations

    public ICollection<Experiment> Experiments { get; set; }

    #endregion
}