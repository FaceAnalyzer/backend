using FaceAnalyzer.Api.Business.Contracts;
using FaceAnalyzer.Api.Service.Contracts;
using FaceAnalyzer.Api.Shared.Enum;
using Swashbuckle.AspNetCore.Filters;

namespace FaceAnalyzer.Api.Service.Swagger.Examples;

public class LoginRequestExample : IExamplesProvider<LoginRequest>
{
    public LoginRequest GetExamples()
    {
        return new LoginRequest(
            "Username",
            "Password"
        );
    }
}