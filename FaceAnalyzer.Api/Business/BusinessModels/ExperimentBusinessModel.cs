using AutoMapper;
using AutoMapper.QueryableExtensions;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Business.UseCases.Experiments;
using FaceAnalyzer.Api.Data;
using FaceAnalyzer.Api.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace FaceAnalyzer.Api.Business.BusinessModels;

public class ExperimentBusinessModel : BusinessModelBase
{
    public ExperimentBusinessModel(IMapper mapper, AppDbContext dbContext) : base(mapper, dbContext)
    {
    }

    public async Task<IList<ExperimentDto>> Get()
    {
        var results = await DbContext.Experiments
            .ProjectTo<ExperimentDto>(Mapper.ConfigurationProvider)
            .ToListAsync();
        return results;
    }

    public async Task<ExperimentDto> Create(ExperimentDto dto)
    {
        var useCase = new CreateExperimentUseCase(Mapper, DbContext);
        return await useCase.Execute(dto);
    }

    public async Task<ExperimentDto> Edit(ExperimentDto dto)
    {
        var useCase = new EditExperimentUseCase(Mapper, DbContext);
        return await useCase.Execute(dto);
    }

    public async Task<ExperimentDto?> Delete(ExperimentDto dto)
    {
        var useCase = new DeleteExperimentUseCase(Mapper, DbContext);
        return await useCase.Execute(dto);
    }
}