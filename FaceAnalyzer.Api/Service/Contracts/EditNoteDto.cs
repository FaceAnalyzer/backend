using Swashbuckle.AspNetCore.Annotations;

namespace FaceAnalyzer.Api.Service.Contracts;

[SwaggerSchema(Title = nameof(EditNoteDto))]
public record EditNoteDto(string Description, int? ExperimentId, int? CreatorId);
