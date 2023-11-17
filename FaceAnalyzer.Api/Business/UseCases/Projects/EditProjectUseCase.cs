using AutoMapper;
using FaceAnalyzer.Api.Business.Commands.Projects;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Data;
using MediatR;

namespace FaceAnalyzer.Api.Business.UseCases.Projects;

public class EditProjectUseCase: BaseUseCase, IRequestHandler<EditProjectCommand, ProjectDto>
{
    public EditProjectUseCase(IMapper mapper, AppDbContext dbContext) : base(mapper, dbContext)
    {
    }

    public async Task<ProjectDto> Handle(EditProjectCommand request, CancellationToken cancellationToken)
    {
        return new ProjectDto(0, "placeholder");
    }
}