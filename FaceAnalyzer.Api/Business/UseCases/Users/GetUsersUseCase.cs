using AutoMapper;
using AutoMapper.QueryableExtensions;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Business.Queries;
using FaceAnalyzer.Api.Data;
using FaceAnalyzer.Api.Data.Entities;
using FaceAnalyzer.Api.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FaceAnalyzer.Api.Business.UseCases.Users;

public class GetUsersUseCase : BaseUseCase, IRequestHandler<GetUsersQuery, QueryResult<UserDto>>
{
    public async Task<QueryResult<UserDto>> Handle(GetUsersQuery request,
        CancellationToken cancellationToken)
    {
        var results = await DbContext.Users
            .ConditionalWhere(request.Id.HasValue, u => u.Id == request.Id)
            .ConditionalWhere(request.Role.HasValue, u => u.Role == request.Role)
            .ConditionalWhere(request.ProjectId.HasValue,
                u => u.Projects.Any(p => p.Id == request.ProjectId)
            )
            .ProjectTo<UserDto>(Mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return results.ToQueryResult();
    }

    public GetUsersUseCase(IMapper mapper, AppDbContext dbContext) : base(mapper, dbContext)
    {
    }
}