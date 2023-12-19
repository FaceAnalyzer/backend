using FaceAnalyzer.Api.Business.Contracts;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;

namespace FaceAnalyzer.Api.Business.Commands.Experiments;

[SwaggerSchema(Title = nameof(EditExperimentCommand))]
public record EditExperimentCommand(int Id, string Name, string Description, int? ProjectId): IRequest<ExperimentDto>;