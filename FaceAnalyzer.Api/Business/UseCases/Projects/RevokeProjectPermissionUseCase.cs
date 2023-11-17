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
        return new ProjectDto(0, "placeholder");
    }
}