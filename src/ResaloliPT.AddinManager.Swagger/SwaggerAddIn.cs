using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ResaloliPT.AddInManager.Abstractions;
using Swashbuckle.AspNetCore.Filters;

namespace ResaloliPT.AddinManager.Swagger
{
    public sealed class SwaggerAddIn : AddIn
    {
        public SwaggerAddIn() : base("ResaloliPT.Swagger")
        { }

        public void ConfigureService(IServiceCollection services, IConfiguration configuration)
        {
            var swaggerOptions = new SwaggerOptions();

            configuration.GetSection(nameof(SwaggerOptions)).Bind(swaggerOptions);

            services.AddSwaggerGen(action =>
            {
                action.SwaggerDoc("v1", new OpenApiInfo { Title = swaggerOptions.ApiName, Version = "v1" });

                action.ExampleFilters();

                if(swaggerOptions.AsSecurity)
                {
                    action.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                    {
                        Description = "JWT Bearer Authorization Header",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey
                    });

                    action.AddSecurityRequirement(new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme{
                                Reference = new OpenApiReference
                                {
                                    Id = "Bearer",
                                    Type = ReferenceType.SecurityScheme
                                }
                            },
                            new List<string>()
                        }
                    });
                }
            });

            var startupType = Assembly
                .GetEntryAssembly()
                .GetExportedTypes()
                .ToList()
                .Where(type => type.Name == "Startup")
                .First();

            MethodInfo method = typeof(IServiceCollection).GetMethod("AddSwaggerExamplesFromAssemblyOf");
            MethodInfo generic = method.MakeGenericMethod(startupType);
            generic.Invoke(this, new[] { services });
        }

        public void Configure(IApplicationBuilder app, IHostEnvironment env, IConfiguration configuration)
        {
            var swaggerOptions = new SwaggerOptions();

            configuration.GetSection(nameof(SwaggerOptions)).Bind(swaggerOptions);

            app.UseSwagger(options =>
            {
                options.RouteTemplate = swaggerOptions.JsonRoute;
            });

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint(swaggerOptions.UIEndpoint, swaggerOptions.ApiName);
            });
        }
    }
}
