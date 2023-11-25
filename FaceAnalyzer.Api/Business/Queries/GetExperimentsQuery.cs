using FaceAnalyzer.Api.Business.Contracts;
using MediatR;

namespace FaceAnalyzer.Api.Business.Queries;

public record GetExperimentsQuery(int? Id, int? ProjectId): IRequest<QueryResult<ExperimentDto>>;