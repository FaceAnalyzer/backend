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

    public async Task<ExperimentDto?> Create(ExperimentDto dto)
    {
        var project = DbContext.Find<Project>(dto.ProjectId);
        if (project is null)
        {
            return null;
        }
        var experiment = Mapper.Map<Experiment>(dto);
        DbContext.Add(experiment);
        await DbContext.SaveChangesAsync();
        return Mapper.Map<ExperimentDto>(experiment);
    }

    public async Task<ExperimentDto?> Edit(ExperimentDto dto)
    {
        var experiment = DbContext.Find<Experiment>(dto.Id);
        if (experiment is null)
        {
            return null;
        }
        experiment.Name = dto.Name;
        experiment.Description = dto.Description;
        experiment.UpdatedAt = DateTime.UtcNow;
        DbContext.Update(experiment);
        await DbContext.SaveChangesAsync();
        return Mapper.Map<ExperimentDto>(experiment);
    }

    public async Task<ExperimentDto?> Delete(ExperimentDto dto)
    {
        var experiment = DbContext.Find<Experiment>(dto.Id);
        if (experiment is null)
        {
            return null;
        }
        experiment.DeletedAt = DateTime.UtcNow;
        DbContext.Update(experiment);
        await DbContext.SaveChangesAsync();
        return Mapper.Map<ExperimentDto>(experiment);
    }
}