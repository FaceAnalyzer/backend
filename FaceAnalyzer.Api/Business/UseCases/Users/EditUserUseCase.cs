using AutoMapper;
using FaceAnalyzer.Api.Business.Commands.Users;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Data;
using FaceAnalyzer.Api.Data.Entities;
using FaceAnalyzer.Api.Shared.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace FaceAnalyzer.Api.Business.UseCases.Users;

public class EditUserUseCase : BaseUseCase, IRequestHandler<EditUserCommand, UserDto>
{
    public EditUserUseCase(IMapper mapper, AppDbContext dbContext) : base(mapper, dbContext)
    {
    }

    public async Task<UserDto> Handle(EditUserCommand request, CancellationToken cancellationToken)
    {
        var exceptionBuilder = new InvalidArgumentsExceptionBuilder();
        var user = await DbContext.Users
            .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

        if (user is null)
        {
            exceptionBuilder
                .AddArgument(nameof(request.Id),
                    $"no user with this id ({request.Id}) was found");
            throw exceptionBuilder.Build();
        }

        if (user.Username != request.Username)
        {
            var userExists = await DbContext.Users.AnyAsync(u => u.Username == request.Username, cancellationToken);
            if (userExists)
            {
                exceptionBuilder
                    .AddArgument(nameof(User.Username), "the username already exist, choose another one");
            }
        }

        if (user.Email != request.Email)
        {
            var userExists = await DbContext.Users.AnyAsync(u => u.Email == request.Email, cancellationToken);
            if (userExists)
            {
                exceptionBuilder
                    .AddArgument(nameof(User.Email), "the email already exist, choose another one");
            }
        }


        if (exceptionBuilder.HasArguments)
        {
            throw exceptionBuilder.Build();
        }


        user.Name = request.Name;
        user.Surname = request.Surname;
        user.Email = request.Email;
        user.Username = request.Username;
        user.ContactNumber = request.ContactNumber;
        user.Role = request.Role;
        
        await DbContext.SaveChangesAsync(cancellationToken);
        return Mapper.Map<UserDto>(user);
    }

    
}