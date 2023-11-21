using AutoMapper;
using FaceAnalyzer.Api.Business.Commands.Users;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Data;
using FaceAnalyzer.Api.Data.Entities;
using FaceAnalyzer.Api.Shared.Exceptions;
using FaceAnalyzer.Api.Shared.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FaceAnalyzer.Api.Business.UseCases.Users;

public class CreateUserUseCase : BaseUseCase, IRequestHandler<CreateUserCommand, UserDto>
{
    private SecurityContext _securityContext;

    public CreateUserUseCase(IMapper mapper, AppDbContext dbContext, SecurityContext securityContext) : base(mapper,
        dbContext)
    {
        _securityContext = securityContext;
    }

    public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var exceptionBuilder = new InvalidArgumentsExceptionBuilder();
        var userExists = await DbContext.Users.AnyAsync(u => u.Username == request.Username, cancellationToken);
        if (userExists)
        {
            exceptionBuilder
                .AddArgument(nameof(User.Username), "the username already exist, choose another one");
        }

        userExists = await DbContext.Users.AnyAsync(u => u.Email == request.Email, cancellationToken);
        if (userExists)
        {
            exceptionBuilder
                .AddArgument(nameof(User.Email), "the email already exist, choose another one");
        }

        if (exceptionBuilder.HasArguments)
        {
            throw exceptionBuilder.Build();
        }

        var user = Mapper.Map<User>(request);
        user.Password = _securityContext.Hash(request.Password);
        DbContext.Add(user);
        await DbContext.SaveChangesAsync(cancellationToken);

        return Mapper.Map<UserDto>(user);
    }
}