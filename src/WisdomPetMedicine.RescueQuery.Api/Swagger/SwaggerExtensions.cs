using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WisdomPetMedicine.RescueQuery.Api.Swagger
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddMultiversionSwagger(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            });
            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
            });
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, SwaggerConfigurationOptions>();
            services.AddSwaggerGen(options =>
            {
                options.OperationFilter<SwaggerOperationFilter>();
            });
            return services;
        }

        public static IApplicationBuilder UseMultiversionSwagger(this IApplicationBuilder app, IApiVersionDescriptionProvider provider)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                foreach (var apiVersionDescription in provider.ApiVersionDescriptions)
                {
                    c.SwaggerEndpoint($"/swagger/{apiVersionDescription.GroupName}/swagger.json", 
                        $"WisdomPetMedicine.RescueQuery.Api {apiVersionDescription.GroupName}");
                }
            });
            return app;
        }
    }
}