using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Shared.Enum;
using MediatR;

namespace FaceAnalyzer.Api.Business.Queries;

public record GetUsersQuery
    : IRequest<QueryResult<UserDto>>
{
    public int? Id { get; init; }
    public int? ProjectId { get; init; }
    public UserRole? Role { get; init; }
}