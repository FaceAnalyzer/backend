using FaceAnalyzer.Api.Business.Contracts;
using MediatR;

namespace FaceAnalyzer.Api.Business.Commands.Experiments;

public record DeleteExperimentCommand(int Id): IRequest<ExperimentDto>;