using AutoMapper;
using FaceAnalyzer.Api.Business.Commands.Users;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Data;
using FaceAnalyzer.Api.Data.Entities;
using FaceAnalyzer.Api.Shared.Exceptions;
using MediatR;

namespace FaceAnalyzer.Api.Business.UseCases.Users;

public class CreateUserUseCase: BaseUseCase, IRequestHandler<CreateUserCommand, UserDto>
{
    public CreateUserUseCase(IMapper mapper, AppDbContext dbContext) : base(mapper, dbContext)
    {
    }

    public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var userExisting1 = DbContext.Users.FirstOrDefault(u => u.Username == request.Username);
        if (userExisting1 is not null)
        {
            throw new InvalidArgumentsExceptionBuilder()
                .AddArgument(nameof(userExisting1.Id),
                    $"the username already exist was found")
                .Build();
        }
        var userExisting2 = DbContext.Users.FirstOrDefault(u => u.Email == request.Email);
        if (userExisting2 is not null)
        {
            throw new InvalidArgumentsExceptionBuilder()
                .AddArgument(nameof(userExisting2.Id),
                    $"the email already exist ")
                .Build();
        }
        var user = Mapper.Map<User>(request);
        DbContext.Add(user);
        await DbContext.SaveChangesAsync(cancellationToken);
        
        return Mapper.Map<UserDto>(user);
    }
}