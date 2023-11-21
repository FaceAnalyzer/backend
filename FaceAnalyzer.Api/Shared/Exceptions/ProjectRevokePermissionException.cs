namespace FaceAnalyzer.Api.Shared.Exceptions;

public class ProjectRevokePermissionException : Exception
{
    public int ResearcherId { get; init; }
    public string ProjectName { get; init; }
    

    public ProjectRevokePermissionException(int researcherId, string projectName)
        : base($"Researcher with id: {researcherId} is not assigned to project: {projectName}")
    {
        ResearcherId = researcherId;
        ProjectName = projectName;
    }
}