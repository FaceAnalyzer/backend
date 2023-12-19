
using MediatR;
using Swashbuckle.AspNetCore.Annotations;

namespace FaceAnalyzer.Api.Business.Contracts;

[SwaggerSchema(Title = nameof(ExperimentDto))]
public record ExperimentDto(int Id, string Name, string Description, int ProjectId);