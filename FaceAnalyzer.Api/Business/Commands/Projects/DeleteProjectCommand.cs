using FaceAnalyzer.Api.Business.Contracts;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;

namespace FaceAnalyzer.Api.Business.Commands.Projects;

[SwaggerSchema(Title = nameof(DeleteProjectCommand))]
public record DeleteProjectCommand(int Id): IRequest;