using AutoMapper;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Data;
using FaceAnalyzer.Api.Data.Entities;

namespace FaceAnalyzer.Api.Business.UseCases.Projects;

public class CreateProjectUseCase: BaseUseCase<ProjectDto, ProjectDto>
{
    
    public override async Task<ProjectDto> Execute(ProjectDto input)
    {
        var project = Mapper.Map<Project>(input);
        DbContext.Projects.Add(project);
        await DbContext.SaveChangesAsync();

        return Mapper.Map<ProjectDto>(project);
    }

    public CreateProjectUseCase(IMapper mapper, AppDbContext dbContext) : base(mapper, dbContext)
    {
    }
}