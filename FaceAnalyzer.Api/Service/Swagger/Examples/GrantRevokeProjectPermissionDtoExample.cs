using FaceAnalyzer.Api.Service.Contracts;
using Swashbuckle.AspNetCore.Filters;

namespace FaceAnalyzer.Api.Service.Swagger.Examples;

public class GrantRevokeProjectPermissionDtoExample: IExamplesProvider<GrantRevokeProjectPermissionDto>
{
    public GrantRevokeProjectPermissionDto GetExamples()
    {
        return new GrantRevokeProjectPermissionDto(ResearchersIds: new List<int> { 1, 2, 3, 44, 31, 100 });
    }
}