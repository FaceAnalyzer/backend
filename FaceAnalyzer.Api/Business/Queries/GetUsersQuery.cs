using FaceAnalyzer.Api.Business.Contracts;
using MediatR;

namespace FaceAnalyzer.Api.Business.Queries;

public record GetUsersQuery(int? Id): IRequest<QueryResult<UserDto>>;