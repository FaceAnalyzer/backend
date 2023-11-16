using System.Security.Authentication;
using AutoMapper;
using FaceAnalyzer.Api.Business.Commands.Auth;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Data;
using FaceAnalyzer.Api.Shared.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FaceAnalyzer.Api.Business.UseCases.Auth;

public class LoginUseCase : BaseUseCase, IRequestHandler<LoginCommand, AuthResult>
{
    private readonly SecurityContext _securityContext;

    public LoginUseCase(IMapper mapper,
        AppDbContext dbContext, SecurityContext securityContext) : base(mapper, dbContext)
    {
        _securityContext = securityContext;
    }

    public async Task<AuthResult> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await DbContext.Users
            .FirstOrDefaultAsync(
                u => u.Username == request.Username,
                cancellationToken);

        if (user is null)
        {
            throw new InvalidCredentialException("Wrong username or password");
        }

        var verified = _securityContext.Compare(
            plainPassword: request.Password,
            hashedPassword: user.Password);

        if (!verified)
        {
            throw new InvalidCredentialException("Wrong username or password");
        }

        var principal = new SecurityPrincipal
        {
            Id = user.Id,
            Role = user.Role
        };

        var result = new AuthResult(
            Id: user.Id,
            AccessToken: _securityContext.CreateJwt(principal)
        );

        return result;
    }
}