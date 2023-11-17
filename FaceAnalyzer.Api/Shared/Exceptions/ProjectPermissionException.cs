namespace FaceAnalyzer.Api.Shared.Exceptions;

public class ProjectPermissionException : Exception
{
    public int ResearcherId { get; init; }
    public string ProjectName { get; init; }

    public ProjectPermissionException(int researcherId, string projectName)
        : base($"Researcher with id: {researcherId} already assigned to project: {projectName}")
    {
        ResearcherId = researcherId;
        ProjectName = projectName;
    }
}