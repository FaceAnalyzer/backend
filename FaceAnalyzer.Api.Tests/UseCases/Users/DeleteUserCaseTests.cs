using FaceAnalyzer.Api.Business.Commands.Users;
using FaceAnalyzer.Api.Business.Profiles;
using FaceAnalyzer.Api.Business.UseCases.Users;
using FaceAnalyzer.Api.Data.Entities;
using FaceAnalyzer.Api.Shared.Exceptions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace FaceAnalyzer.Api.Tests.UseCases.Users;

public class DeleteUserUseCaseTests
{
    [Fact(DisplayName = "Successfully Delete an existing user")]
    public async Task Should_Delete_User_In_DB()
    {
        // Arrange
        var services =
            new IsolatedUseCaseTestServices<DeleteUserUseCase>("DeleteUserUseCaseTests",
                new UserMappingProfile());
        var useCase = services.UseCase;
        var dbContext = services.DbContext;

        var user = new User
        {
            Name = "Dummy user",
        };
        dbContext.Users.Add(user);
        await dbContext.SaveChangesAsync();

        var request = new DeleteUserCommand(user.Id);

        // Act
        await useCase.Handle(request, CancellationToken.None);

        // Assert
        var deletedUser = await dbContext.Users
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(p => p.Id == user.Id);

        // Assertion
        deletedUser.DeletedAt.Should().NotBeNull();
    }

    [Fact(DisplayName = "Throw InvalidArgumentsException when user does not exist")]
    public async Task Should_Throw_InvalidArgumentsException_When_User_Does_Not_Exist()
    {
        // Arrange
        var services =
            new IsolatedUseCaseTestServices<DeleteUserUseCase>("DeleteUserUseCaseTests",
                new UserMappingProfile());
        var useCase = services.UseCase;

        // Assuming there is no user with Id 999
        var nonExistingUserId = 999;
        var request = new DeleteUserCommand(nonExistingUserId);

        // Act
        Func<Task> act = async () => await useCase.Handle(request, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<InvalidArgumentsException>()
            .WithMessage($"Id: no user with this id ({request.Id}) was found");
    }
    
}