using FaceAnalyzer.Api.Business.Contracts;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;

namespace FaceAnalyzer.Api.Business.Commands.Projects;

[SwaggerSchema(Title = nameof(CreateProjectCommand))]
public record CreateProjectCommand(string Name): IRequest<ProjectDto>;