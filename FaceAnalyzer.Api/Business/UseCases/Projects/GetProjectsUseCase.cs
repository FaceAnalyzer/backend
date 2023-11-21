using AutoMapper;
using AutoMapper.QueryableExtensions;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Business.Queries;
using FaceAnalyzer.Api.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace FaceAnalyzer.Api.Business.UseCases.Projects;

public class GetProjectsUseCase: BaseUseCase, IRequestHandler<GetProjectsQuery, QueryResult<ProjectDto>>
{
    public GetProjectsUseCase(IMapper mapper, AppDbContext dbContext) : base(mapper, dbContext)
    {
    }

    public async Task<QueryResult<ProjectDto>> Handle(GetProjectsQuery request, CancellationToken cancellationToken)
    {
        var result = await DbContext.Projects
            .ConditionalWhere(request.Id.HasValue, project => project.Id == request.Id)
            .ConditionalWhere(!request.Name.IsNullOrEmpty(), project => project.Name == request.Name)
            .ProjectTo<ProjectDto>(Mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
        return result.ToQueryResult();
    }
}