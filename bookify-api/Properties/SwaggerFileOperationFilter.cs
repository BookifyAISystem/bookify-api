using bookify_data.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace bookify_api.Properties
{
	public class SwaggerFileOperationFilter : IOperationFilter
	{
		public void Apply(OpenApiOperation operation, OperationFilterContext context)
		{
			if (operation == null || context == null) return;

			var hasFileParameter = context.MethodInfo.GetParameters()
				.Any(p => IsFileUploadType(p.ParameterType));

			if (!hasFileParameter) return;

			if (operation.RequestBody == null)
			{
				operation.RequestBody = new OpenApiRequestBody();
			}

			operation.RequestBody.Content = new Dictionary<string, OpenApiMediaType>
			{
				["multipart/form-data"] = new OpenApiMediaType
				{
					Schema = new OpenApiSchema
					{
						Type = "object",
						Properties = new Dictionary<string, OpenApiSchema>
						{
							["models"] = new OpenApiSchema
							{
								Type = "array",
								Items = new OpenApiSchema
								{
									Type = "object",
									Properties = new Dictionary<string, OpenApiSchema>
									{
										["certificationName"] = new OpenApiSchema
										{
											Type = "string",
											Description = "Name of the certification"
										},
										["certifications"] = new OpenApiSchema
										{
											Type = "array",
											Items = new OpenApiSchema
											{
												Type = "string",
												Format = "binary",
												Description = "Certification files"
											}
										}
									},
									Required = new HashSet<string> { "certificationName", "certifications" }
								}
							}
						}
					}
				}
			};

		}

		private bool IsFileUploadType(Type type)
		{
			if (type == null) return false;

			if (type == typeof(IFormFile) || type == typeof(List<IFormFile>))
				return true;

			if (type == typeof(UpdateCertification) ||
				(type.IsGenericType &&
				 type.GetGenericTypeDefinition() == typeof(List<>) &&
				 type.GetGenericArguments()[0] == typeof(UpdateCertification)))
				return true;

			return false;
		}
	}
}
