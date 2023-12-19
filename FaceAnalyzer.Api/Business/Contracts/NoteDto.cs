using Swashbuckle.AspNetCore.Annotations;

namespace FaceAnalyzer.Api.Business.Contracts;

[SwaggerSchema(Title = nameof(NoteDto))]
public record NoteDto(int Id, string Description, int ExperimentId, int CreatorId);