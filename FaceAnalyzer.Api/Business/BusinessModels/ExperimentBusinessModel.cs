using AutoMapper;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Data;
using FaceAnalyzer.Api.Data.Entities;

namespace FaceAnalyzer.Api.Business.BusinessModels;

public class ExperimentBusinessModel: BusinessModelBase
{
    
    public ExperimentBusinessModel(IMapper mapper, AppDbContext dbContext) : base(mapper, dbContext)
    {
    }

    public async Task<ExperimentDto> Create(ExperimentDto dto)
    {
        DbContext.Add(Mapper.Map<Experiment>(dto));
        await DbContext.SaveChangesAsync();
        return dto;
    }

    public async Task<ExperimentDto?> Edit(ExperimentDto dto)
    {
        var oldExperiment = DbContext.Find<Experiment>(dto.Id);
        if (oldExperiment is null)
        {
            return null;
        }
        oldExperiment.Name = dto.Name;
        oldExperiment.Description = dto.Description;
        DbContext.Update(oldExperiment);
        await DbContext.SaveChangesAsync();
        return dto;
    }

    public async Task<ExperimentDto?> Delete(ExperimentDto dto)
    {
        var oldExperiment = DbContext.Find<Experiment>(dto.Id);
        if (oldExperiment is null)
        {
            return null;
        }
        oldExperiment.DeletedAt = DateTime.UtcNow;
        DbContext.Update(oldExperiment);
        await DbContext.SaveChangesAsync();
        return dto;
    }
}