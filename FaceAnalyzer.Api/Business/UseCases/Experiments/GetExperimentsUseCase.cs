using AutoMapper;
using AutoMapper.QueryableExtensions;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Business.Queries;
using FaceAnalyzer.Api.Data;
using FaceAnalyzer.Api.Data.Entities;
using FaceAnalyzer.Api.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FaceAnalyzer.Api.Business.UseCases.Experiments;

public class GetExperimentsUseCase : BaseUseCase, IRequestHandler<GetExperimentsQuery, QueryResult<ExperimentDto>>
{
    public async Task<QueryResult<ExperimentDto>> Handle(GetExperimentsQuery request,
        CancellationToken cancellationToken)
    {
        var results = await DbContext.Experiments
            .ConditionalWhere(request.Id.HasValue, e=> e.Id == request.Id)
            .ProjectTo<ExperimentDto>(Mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
        return results.ToQueryResult();
    }

    public GetExperimentsUseCase(IMapper mapper, AppDbContext dbContext) : base(mapper, dbContext)
    {
    }
}