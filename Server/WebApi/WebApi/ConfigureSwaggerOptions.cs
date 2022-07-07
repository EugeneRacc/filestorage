using System;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebApi
{
    /// <summary>
    /// Configuration for swagger
    /// </summary>
    /// <seealso cref="Microsoft.Extensions.Options.IConfigureOptions&lt;Swashbuckle.AspNetCore.SwaggerGen.SwaggerGenOptions&gt;" />
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;
        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) => _provider = provider;
        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                var apiVersion = description.ApiVersion.ToString();
                options.SwaggerDoc(description.GroupName,
                    new OpenApiInfo
                    {
                        Version = apiVersion,
                        Title = $"FileStorage API {apiVersion}",
                        Description = "FileStorage swagger with versioning in work",
                        TermsOfService = new Uri($"https://google.com"),
                        Contact = new OpenApiContact
                        {
                            Name = "FileStorage",
                            Email = "forgbracc@gmail.com",
                            Url = new Uri($"https://google.com")
                        },
                        License = new OpenApiLicense
                        {
                            Name = "FileStorage",
                            Url = new Uri($"https://google.com")
                            
                        }
                        
                    });
                options.AddSecurityDefinition($"AuthToken {apiVersion}", 
                    new OpenApiSecurityScheme
                    {
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.Http,
                        BearerFormat = "JWT",
                        Scheme = "bearer",
                        Name = "Authorization",
                        Description = "Authorization token"
                    });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = $"AuthToken {apiVersion}"
                            }
                        },
                        new string[] {}
                    }
                });
                options.CustomOperationIds(apiDescription => 
                    apiDescription.TryGetMethodInfo(out MethodInfo methodInfo)
                    ? methodInfo.Name
                    : null);
            }
        }
    }
}