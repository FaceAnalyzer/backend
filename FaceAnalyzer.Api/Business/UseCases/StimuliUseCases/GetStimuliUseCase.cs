using AutoMapper;
using AutoMapper.QueryableExtensions;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Business.Queries;
using FaceAnalyzer.Api.Data;
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
                .ProjectTo<StimuliDto>(Mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(s => s.Id == request.Id.Value, cancellationToken);
            if (stimuli is null)
            {
                throw new Exception("Stimuli not found");
            }

            result.Add(stimuli);
        }

        result = await DbContext.Stimuli
            .ProjectTo<StimuliDto>(Mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return result.ToQueryResult();
    }

    public GetStimuliUseCase(IMapper mapper, AppDbContext dbContext) : base(mapper, dbContext)
    {
    }
}