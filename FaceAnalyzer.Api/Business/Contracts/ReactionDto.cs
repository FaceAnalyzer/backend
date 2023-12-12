using FaceAnalyzer.Api.Data.Entities;
using Swashbuckle.AspNetCore.Annotations;

namespace FaceAnalyzer.Api.Business.Contracts;

[SwaggerSchema(Title = nameof(ReactionDto))]
public record ReactionDto(int Id, int StimuliId, string ParticipantName);