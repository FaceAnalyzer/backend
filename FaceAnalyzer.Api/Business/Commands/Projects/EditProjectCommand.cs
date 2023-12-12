using FaceAnalyzer.Api.Business.Contracts;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;

namespace FaceAnalyzer.Api.Business.Commands.Projects;

[SwaggerSchema(Title = nameof(EditProjectCommand))]
public record EditProjectCommand(int Id, string Name): IRequest<ProjectDto>;