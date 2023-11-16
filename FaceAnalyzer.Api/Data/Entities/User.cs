using FaceAnalyzer.Api.Shared.Enum;

namespace FaceAnalyzer.Api.Data.Entities;

public class User : EntityBase, IDeletable
{
    public string Name { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? ContactNumber { get; set; }
    public UserRole Role { get; set; }

    public ICollection<Project> Projects { get; set; } = new List<Project>();
    public DateTime? DeletedAt { get; set; }

    public void Delete()
    {
        Projects.Clear();
        DeletedAt = DateTime.Now;
    }
}