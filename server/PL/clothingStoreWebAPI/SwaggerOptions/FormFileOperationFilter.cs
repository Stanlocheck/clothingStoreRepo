using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public class FormFileOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var parameters = context.MethodInfo.GetParameters();

        // Проверяем, есть ли параметры с типом IFormFile
        if (!parameters.Any(p => p.ParameterType == typeof(IFormFile) || p.ParameterType == typeof(IEnumerable<IFormFile>)))
        {
            return;
        }

        // Удаляем существующие параметры из операции
        operation.RequestBody = new OpenApiRequestBody
        {
            Content =
            {
                ["multipart/form-data"] = new OpenApiMediaType
                {
                    Schema = new OpenApiSchema
                    {
                        Type = "object",
                        Properties = new Dictionary<string, OpenApiSchema>()
                    },
                    Encoding = new Dictionary<string, OpenApiEncoding>()
                }
            }
        };

        foreach (var parameter in parameters)
        {
            if (parameter.ParameterType == typeof(IFormFile))
            {
                operation.RequestBody.Content["multipart/form-data"].Schema.Properties.Add(parameter.Name, new OpenApiSchema
                {
                    Type = "string",
                    Format = "binary"
                });

                operation.RequestBody.Content["multipart/form-data"].Encoding.Add(parameter.Name, new OpenApiEncoding
                {
                    ContentType = "application/octet-stream"
                });
            }
            else if (parameter.ParameterType == typeof(IEnumerable<IFormFile>))
            {
                operation.RequestBody.Content["multipart/form-data"].Schema.Properties.Add(parameter.Name, new OpenApiSchema
                {
                    Type = "array",
                    Items = new OpenApiSchema
                    {
                        Type = "string",
                        Format = "binary"
                    }
                });

                operation.RequestBody.Content["multipart/form-data"].Encoding.Add(parameter.Name, new OpenApiEncoding
                {
                    ContentType = "application/octet-stream"
                });
            }
            else
            {
                operation.RequestBody.Content["multipart/form-data"].Schema.Properties.Add(parameter.Name, new OpenApiSchema
                {
                    Type = "string"
                });
            }
        }
    }
}
