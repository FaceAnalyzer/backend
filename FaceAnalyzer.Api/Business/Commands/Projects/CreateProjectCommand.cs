using FaceAnalyzer.Api.Business.Contracts;
using MediatR;

namespace FaceAnalyzer.Api.Business.Commands.Projects;

public record CreateProjectCommand(string Name): IRequest<ProjectDto>;