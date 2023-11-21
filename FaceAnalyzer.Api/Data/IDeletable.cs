namespace FaceAnalyzer.Api.Data;

public interface IDeletable
{
    public DateTime? DeletedAt { get; set; }

    public void Delete();
}