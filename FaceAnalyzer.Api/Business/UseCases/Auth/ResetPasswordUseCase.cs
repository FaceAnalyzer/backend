using AutoMapper;
using FaceAnalyzer.Api.Business.Commands.Auth;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Data;
using FaceAnalyzer.Api.Data.Entities;
using FaceAnalyzer.Api.Shared.Exceptions;
using FaceAnalyzer.Api.Shared.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FaceAnalyzer.Api.Business.UseCases.Auth;

public class ResetPasswordUseCase : BaseUseCase, IRequestHandler<ResetPasswordCommand, ResetPasswordResult>
{
    private readonly SecurityContext _securityContext;

    public ResetPasswordUseCase(IMapper mapper, AppDbContext dbContext, SecurityContext securityContext) : base(mapper,
        dbContext)
    {
        _securityContext = securityContext;
    }

    public async Task<ResetPasswordResult> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await DbContext.Users
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user is null)
        {
            throw new EntityNotFoundException(nameof(User), request.UserId);
        }

        var newPasswordHash = _securityContext.Hash(request.NewPassword);
        user.Password = newPasswordHash;

        await DbContext.SaveChangesAsync(cancellationToken);

        return new ResetPasswordResult(user.Id);
    }
}