using FaceAnalyzer.Api.Business.Contracts;
using MediatR;


namespace FaceAnalyzer.Api.Business.Queries;

public record GetReactionsQuery(int? Id): IRequest<QueryResult<ReactionDto>>;