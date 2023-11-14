namespace FaceAnalyzer.Api.Data.Entities;

public class User: EntityBase
{
    public required string Firstname { get; set; }
    public required string Lastname   { get; set; }

    public ICollection<Project> Projects { get; set; }
}