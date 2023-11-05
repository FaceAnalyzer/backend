using FaceAnalyzer.Api.Business.Contracts;
using MediatR;

namespace FaceAnalyzer.Api.Business.Queries;

public record GetReactionEmotionsQuery(int ReactionId): IRequest<QueryResult<EmotionDto>>;