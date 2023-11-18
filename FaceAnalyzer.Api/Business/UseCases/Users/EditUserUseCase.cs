using AutoMapper;
using FaceAnalyzer.Api.Business.Commands.Users;
using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Data;
using FaceAnalyzer.Api.Data.Entities;
using FaceAnalyzer.Api.Shared.Exceptions;
using MediatR;


namespace FaceAnalyzer.Api.Business.UseCases.Users;

public class EditUserUseCase : BaseUseCase, IRequestHandler<EditUserCommand, UserDto>
{
    public EditUserUseCase(IMapper mapper, AppDbContext dbContext) : base(mapper, dbContext)
    {
    }

    public async Task<UserDto> Handle(EditUserCommand request, CancellationToken cancellationToken)
    {
        var user = DbContext.Find<User>(request.Id);
        if (user is null)
        {
            throw new InvalidArgumentsExceptionBuilder()
                .AddArgument(nameof(request.Id),
                    $"no experiment with this id ({request.Id}) was found")
                .Build();
        }
        var userExisting1 = DbContext.Users.FirstOrDefault(u => u.Username == request.Username);
        if (userExisting1 is not null && userExisting1.Id != request.Id)
        {
            throw new InvalidArgumentsException("the username already exist, choose another one");
        }
        var userExisting2 = DbContext.Users.FirstOrDefault(u => u.Email == request.Email);
        if (userExisting2 is not null && userExisting2.Id != request.Id)
        {
            throw new InvalidArgumentsException("the email already exist, choose another one");
        }

        

        user.Name = request.Name;
        user.Surname = request.Surname;
        user.Email = request.Email;
        user.Username = request.Username;
        user.ContactNumber = request.ContactNumber;
        user.Role = request.Role;
        user.UpdatedAt = DateTime.UtcNow;
        DbContext.Update(user);
        await DbContext.SaveChangesAsync(cancellationToken);
        user.Password = null;
        return Mapper.Map<UserDto>(user);
    }
    
}