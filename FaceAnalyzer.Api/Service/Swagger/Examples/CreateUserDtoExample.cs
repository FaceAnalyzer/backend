using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Service.Contracts;
using FaceAnalyzer.Api.Shared.Enum;
using Swashbuckle.AspNetCore.Filters;

namespace FaceAnalyzer.Api.Service.Swagger.Examples;

public class CreateUserDtoExample : IExamplesProvider<CreateUserDto>
{
    public CreateUserDto GetExamples()
    {
        return new CreateUserDto(
            "First Name",
            "Last Name",
            "email@example.com",
            "Username",
            "Password",
            "+11234567890",
            UserRole.Researcher
        );
    }
}