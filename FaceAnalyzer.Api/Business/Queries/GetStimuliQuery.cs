using FaceAnalyzer.Api.Business.Contracts;
using MediatR;

namespace FaceAnalyzer.Api.Business.Queries;

public class GetStimuliQuery : IRequest<QueryResult<StimuliDto>>
{
    public int? Id { get; init; }

    public int? ReactionId { get; init; }
}