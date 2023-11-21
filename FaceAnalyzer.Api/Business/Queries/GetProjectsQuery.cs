using FaceAnalyzer.Api.Business.Contracts;
using MediatR;

namespace FaceAnalyzer.Api.Business.Queries;

public record GetProjectsQuery(int? Id, string? Name): IRequest<QueryResult<ProjectDto>>;