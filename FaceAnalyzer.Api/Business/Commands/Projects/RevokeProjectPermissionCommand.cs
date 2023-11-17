using FaceAnalyzer.Api.Business.Contracts;
using MediatR;

namespace FaceAnalyzer.Api.Business.Commands.Projects;

public record RevokeProjectPermissionCommand(int ProjectId, List<int> ResearcherIds): IRequest<ProjectDto>;