using FaceAnalyzer.Api.Business.Contracts;
using MediatR;
using Swashbuckle.AspNetCore.Annotations;

namespace FaceAnalyzer.Api.Business.Commands.Experiments;

[SwaggerSchema(Title = nameof(DeleteExperimentCommand))]
public record DeleteExperimentCommand(int Id): IRequest;