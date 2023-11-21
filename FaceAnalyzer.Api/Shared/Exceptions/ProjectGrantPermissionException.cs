namespace FaceAnalyzer.Api.Shared.Exceptions;

public class ProjectGrantPermissionException : Exception
{
    public int ResearcherId { get; init; }
    public string ProjectName { get; init; }
    

    public ProjectGrantPermissionException(int researcherId, string projectName)
        : base($"Researcher with id: {researcherId} already assigned to project: {projectName}")
    {
        ResearcherId = researcherId;
        ProjectName = projectName;
    }
}