using FaceAnalyzer.Api.Business.Contracts;
using MediatR;

namespace FaceAnalyzer.Api.Business.Commands.Projects;

public record EditProjectCommand(int Id, string Name): IRequest<ProjectDto>;