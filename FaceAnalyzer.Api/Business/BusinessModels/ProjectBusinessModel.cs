using AutoMapper;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Data;
using FaceAnalyzer.Api.Data.Entities;

namespace FaceAnalyzer.Api.Business.BusinessModels;

public class ProjectBusinessModel : BusinessModelBase
{
    public ProjectBusinessModel(IMapper mapper, AppDbContext dbContext) : base(mapper, dbContext)
    {
    }

    public async Task<ProjectDto> Create(ProjectDto projectDto)
    {
        var project = Mapper.Map<Project>(projectDto);
        DbContext.Projects.Add(project);
        await DbContext.SaveChangesAsync();

        return Mapper.Map<ProjectDto>(project);
    }
}