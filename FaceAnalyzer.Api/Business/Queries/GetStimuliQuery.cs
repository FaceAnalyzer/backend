using FaceAnalyzer.Api.Business.Contracts;
using MediatR;

namespace FaceAnalyzer.Api.Business.Queries;

public record GetStimuliQuery(int? Id): IRequest<QueryResult<StimuliDto>>;