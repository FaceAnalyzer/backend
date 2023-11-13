using AutoMapper;
using AutoMapper.QueryableExtensions;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Business.Queries;
using FaceAnalyzer.Api.Data;
using FaceAnalyzer.Api.Data.Entities;
using FaceAnalyzer.Api.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FaceAnalyzer.Api.Business.UseCases.StimuliUseCases;

public class GetStimuliUseCase : BaseUseCase, IRequestHandler<GetStimuliQuery, QueryResult<StimuliDto>>
{
    public async Task<QueryResult<StimuliDto>> Handle(GetStimuliQuery request, CancellationToken cancellationToken)
    {
        var result = new List<StimuliDto>();
        if (request.Id.HasValue)
        {
            var stimuli = await DbContext.Stimuli
                .FirstOrDefaultAsync(s => s.Id == request.Id.Value,
                    cancellationToken);
            if (stimuli is null)
            {
                throw new EntityNotFoundException(nameof(Stimuli), request.Id.Value);
            }

            result.Add(Mapper.Map<StimuliDto>(stimuli));
        }
        else
        {
            result = await DbContext.Stimuli
                .ConditionalWhere(request.ExperimentId.HasValue,
                    s => s.ExperimentId == request.ExperimentId.Value)
                .ProjectTo<StimuliDto>(Mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }

        return result.ToQueryResult();
    }

    public GetStimuliUseCase(IMapper mapper, AppDbContext dbContext) : base(mapper, dbContext)
    {
    }
}