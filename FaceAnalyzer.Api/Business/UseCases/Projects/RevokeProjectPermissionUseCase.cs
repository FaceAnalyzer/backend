using AutoMapper;
using FaceAnalyzer.Api.Business.Commands.Projects;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Data;
using FaceAnalyzer.Api.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace FaceAnalyzer.Api.Business.UseCases.Projects;

public class RevokeProjectPermissionUseCase : BaseUseCase, IRequestHandler<RevokeProjectPermissionCommand, ProjectDto>
{
    public RevokeProjectPermissionUseCase(IMapper mapper, AppDbContext dbContext) : base(mapper, dbContext)
    {
    }

    public async Task<ProjectDto> Handle(RevokeProjectPermissionCommand request, CancellationToken cancellationToken)
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

        foreach (var researcherId in request.ResearcherIds)
        {
            var researcher = project.Users.FirstOrDefault(user => user.Id == researcherId);
            if(researcher is null) throw new ProjectRevokePermissionException(researcherId, project.Name);
            project.Users.Remove(researcher);
        }
        
        await DbContext.SaveChangesAsync(cancellationToken);
        return Mapper.Map<ProjectDto>(project);
    }
}