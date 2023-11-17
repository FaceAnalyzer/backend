using AutoMapper;
using FaceAnalyzer.Api.Business.Commands.Projects;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Data;
using FaceAnalyzer.Api.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace FaceAnalyzer.Api.Business.UseCases.Projects;

public class GrantProjectPermissionUseCase : BaseUseCase, IRequestHandler<GrantProjectPermissionCommand, ProjectDto>
{
    public GrantProjectPermissionUseCase(IMapper mapper, AppDbContext dbContext) : base(mapper, dbContext)
    {
    }

    public async Task<ProjectDto> Handle(GrantProjectPermissionCommand request, CancellationToken cancellationToken)
    {
        var project = await DbContext.Projects
            .Include(project => project.Users)
            .FirstOrDefaultAsync(project => project.Id == request.ProjectId, cancellationToken);
        if (project is null)
        {
            throw new InvalidArgumentsExceptionBuilder()
                .AddArgument(nameof(request.ProjectId),
                    $"No project with this id {request.ProjectId} was found")
                .Build();
        }

        foreach (var researcherId in request.ResearcherIds.Where(id => project.Users.Any(user => user.Id == id)))
        {
            throw new ProjectPermissionException(researcherId, project.Name);
        }
        
        var researchers = DbContext.Users.Where(user => request.ResearcherIds.Contains(user.Id));
        foreach (var researcher in researchers)
        {
            project.Users.Add(researcher);
        }
        
        await DbContext.SaveChangesAsync(cancellationToken);
        return Mapper.Map<ProjectDto>(project);
    }
}