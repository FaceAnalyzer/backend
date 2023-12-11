using FaceAnalyzer.Api.Business.Contracts;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;

namespace FaceAnalyzer.Api.Business.Commands.Experiments;

[SwaggerSchema(Title = nameof(CreateExperimentCommand))]
public record CreateExperimentCommand(string Name, string Description, int ProjectId): IRequest<ExperimentDto>;