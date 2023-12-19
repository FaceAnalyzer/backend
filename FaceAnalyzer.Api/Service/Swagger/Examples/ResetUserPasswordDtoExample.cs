using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Service.Contracts;
using FaceAnalyzer.Api.Shared.Enum;
using Swashbuckle.AspNetCore.Filters;

namespace FaceAnalyzer.Api.Service.Swagger.Examples;

public class ResetUserPasswordDtoExample : IExamplesProvider<ResetUserPasswordDto>
{
    public ResetUserPasswordDto GetExamples()
    {
        return new ResetUserPasswordDto(
            1,
            "Password"
        );
    }
}