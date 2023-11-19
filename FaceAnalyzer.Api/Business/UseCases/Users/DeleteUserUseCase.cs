using AutoMapper;
using FaceAnalyzer.Api.Business.Commands.Users;
using FaceAnalyzer.Api.Data;
using FaceAnalyzer.Api.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FaceAnalyzer.Api.Business.UseCases.Users;

public class DeleteUserUseCase: BaseUseCase, IRequestHandler<DeleteUserCommand>
{
    public DeleteUserUseCase(IMapper mapper, AppDbContext dbContext) : base(mapper, dbContext)
    {
    }
    
    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await DbContext.Users
            .Include(u=> u.Projects)
            .AsSplitQuery()
            .FirstOrDefaultAsync(r => r.Id == request.Id, cancellationToken);

        if (user is null)
        {
            throw new InvalidArgumentsExceptionBuilder()
                .AddArgument(nameof(request.Id),
                    $"no user with this id ({request.Id}) was found")
                .Build();
        }

        DbContext.Delete(user);
        await DbContext.SaveChangesAsync(cancellationToken);
    }
    
}