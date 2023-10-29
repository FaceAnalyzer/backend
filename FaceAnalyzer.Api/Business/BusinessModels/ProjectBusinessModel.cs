using AutoMapper;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Business.UseCases.Projects;
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
        var useCase = new CreateProjectUseCase(Mapper, DbContext);
        return await useCase.Execute(projectDto);
    }
}