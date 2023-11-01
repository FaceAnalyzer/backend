using FaceAnalyzer.Api.Business.Contracts;
using MediatR;

namespace FaceAnalyzer.Api.Business.Queries;

public record GetExperimentsQuery(int? Id): IRequest<QueryResult<ExperimentDto>>;