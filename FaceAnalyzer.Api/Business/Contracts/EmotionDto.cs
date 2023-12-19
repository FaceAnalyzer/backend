using FaceAnalyzer.Api.Data.Entities;
using FaceAnalyzer.Api.Shared.Enum;
using Swashbuckle.AspNetCore.Annotations;

namespace FaceAnalyzer.Api.Business.Contracts;

[SwaggerSchema(Title = nameof(EmotionDto))]
public record EmotionDto(int Id, double Value, long TimeOffset, EmotionType EmotionType, int ReactionId);
