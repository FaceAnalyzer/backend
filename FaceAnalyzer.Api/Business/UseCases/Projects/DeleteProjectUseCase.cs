using AutoMapper;
using FaceAnalyzer.Api.Business.Commands.Projects;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Data;
using FaceAnalyzer.Api.Data.Entities;
using FaceAnalyzer.Api.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FaceAnalyzer.Api.Business.UseCases.Projects;

public class DeleteProjectUseCase: BaseUseCase, IRequestHandler<DeleteProjectCommand>
{
    public DeleteProjectUseCase(IMapper mapper, AppDbContext dbContext) : base(mapper, dbContext)
    {
    }

    public async Task Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        var project = await DbContext.Projects
            .Include(p => p.Users)
            .Include(p => p.Experiments)
            .ThenInclude(r => r.Stimuli)
            .ThenInclude(s => s.Reactions)
            .ThenInclude(r => r.Emotions)
            .AsSplitQuery()
            .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);

        if (project is null)
        {
            throw new InvalidArgumentsExceptionBuilder()
                .AddArgument(nameof(request.Id),
                    $"no project with this id ({request.Id}) was found")
                .Build();
        }
        
        DbContext.Delete(project);
        await DbContext.SaveChangesAsync(cancellationToken);
    }
}