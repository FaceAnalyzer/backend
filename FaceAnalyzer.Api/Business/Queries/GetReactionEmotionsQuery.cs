using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Shared.Enum;
using MediatR;

namespace FaceAnalyzer.Api.Business.Queries;

public record GetReactionEmotionsQuery(int ReactionId, EmotionType? EmotionType): IRequest<QueryResult<EmotionDto>>;