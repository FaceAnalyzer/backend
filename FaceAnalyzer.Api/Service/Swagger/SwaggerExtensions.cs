using System.Reflection;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FaceAnalyzer.Api.Service.Swagger;

public static class SwaggerExtensions
{
    public static IEnumerable<T> GetControllerAndActionAttributes<T>(this OperationFilterContext context)
        where T : Attribute
    {
        var result = new List<T>();

        if (context.MethodInfo != null)
        {
            var controllerAttributes = context.MethodInfo.ReflectedType?.GetTypeInfo().GetCustomAttributes<T>();
            result.AddRange(controllerAttributes);

            var actionAttributes = context.MethodInfo.GetCustomAttributes<T>();
            result.AddRange(actionAttributes);
        }

        return result;
    }
}