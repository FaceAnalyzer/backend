using FaceAnalyzer.Api.Business.Contracts;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;

namespace FaceAnalyzer.Api.Business.Commands.Projects;

[SwaggerSchema(Title = nameof(RevokeProjectPermissionCommand))]
public record RevokeProjectPermissionCommand(int ProjectId, List<int> ResearcherIds): IRequest<ProjectDto>;