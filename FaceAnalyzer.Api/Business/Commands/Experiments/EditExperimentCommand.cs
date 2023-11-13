using FaceAnalyzer.Api.Business.Contracts;
using MediatR;

namespace FaceAnalyzer.Api.Business.Commands.Experiments;

public record EditExperimentCommand(int Id, string Name, string Description, int? ProjectId): IRequest<ExperimentDto>;