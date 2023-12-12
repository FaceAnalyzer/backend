using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Service.Contracts;
using FaceAnalyzer.Api.Shared.Enum;
using Swashbuckle.AspNetCore.Filters;

namespace FaceAnalyzer.Api.Service.Swagger.Examples;

public class EditUserDtoExample : IExamplesProvider<EditUserDto>
{
    public EditUserDto GetExamples()
    {
        return new EditUserDto(
            "First Name",
            "Last Name",
            "email@example.com",
            "Username",
            "+11234567890",
            UserRole.Researcher
        );
    }
}