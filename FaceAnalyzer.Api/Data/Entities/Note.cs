using Swashbuckle.AspNetCore.Annotations;

namespace FaceAnalyzer.Api.Data.Entities;

[SwaggerSchema(Title = "Note")]
public class Note : EntityBase, IDeletable
{
    
    public required string Description { get; set; }
    
    public required int ExperimentId { get; set; }

    public required int CreatorId { get; set; }

    public Experiment Experiment { get; set; }
    
    public User Creator { get; set; }
    public DateTime? DeletedAt { get; set; }
    
    public void Delete()
    {
        DeletedAt = DateTime.Now;
    }
    
}