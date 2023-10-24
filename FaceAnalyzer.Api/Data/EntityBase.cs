using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FaceAnalyzer.Api.Data;

public abstract class EntityBase
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    [DefaultValue("NOW()")]
    
    public DateTime CreatedAt { get; set; }
    
    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
    [DefaultValue("NOW()")]
    public DateTime UpdatedAt { get; set; }
}