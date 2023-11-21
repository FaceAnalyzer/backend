using AutoMapper;
using FaceAnalyzer.Api.Business.Commands.Projects;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Data;
using FaceAnalyzer.Api.Shared.Exceptions;
using MediatR;

namespace FaceAnalyzer.Api.Business.UseCases.Projects;

public class EditProjectUseCase: BaseUseCase, IRequestHandler<EditProjectCommand, ProjectDto>
{
    public EditProjectUseCase(IMapper mapper, AppDbContext dbContext) : base(mapper, dbContext)
    {
    }

    public async Task<ProjectDto> Handle(EditProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await DbContext.Projects.FindAsync(request.Id, cancellationToken);
        if (project is null)
        {
            throw new InvalidArgumentsException($"Project with Id: {request.Id} does not exist");
        }

        project.Name = request.Name;
        await DbContext.SaveChangesAsync(cancellationToken);
        return Mapper.Map<ProjectDto>(project);
    }
}