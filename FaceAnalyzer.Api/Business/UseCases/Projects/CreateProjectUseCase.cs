using AutoMapper;
using FaceAnalyzer.Api.Business.Commands.Projects;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Data;
using FaceAnalyzer.Api.Data.Entities;
using MediatR;

namespace FaceAnalyzer.Api.Business.UseCases.Projects;

public class CreateProjectUseCase: BaseUseCase, IRequestHandler<CreateProjectCommand, ProjectDto>
{

    public CreateProjectUseCase(IMapper mapper, AppDbContext dbContext) : base(mapper, dbContext)
    {
    }

    public async Task<ProjectDto> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        var project = Mapper.Map<Project>(request);
        DbContext.Projects.Add(project);
        await DbContext.SaveChangesAsync(cancellationToken);

        return Mapper.Map<ProjectDto>(project);
    }
}