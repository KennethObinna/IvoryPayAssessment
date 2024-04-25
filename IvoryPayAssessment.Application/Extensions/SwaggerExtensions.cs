using IvoryPayAssessment.Application.Common.Filters;
using IvoryPayAssessment.Application.Common.Helpers;

namespace IvoryPayAssessment.Application.Extensions
{
    public static class SwaggerExtensions
    {

        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
        {
            XmlDocument.AddSwaggerXmlCommentsHelper(c);
            c.OperationFilter<SwaggerDefaultValues>();
            c.OperationFilter<SwaggerDefaultValue>();
          //  c.OperationFilter<ReApplyOptionalRouteParameterOperationFilter>();

            c.DocumentFilter<LowerCaseDocumentFilter>();
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
            {
                In = ParameterLocation.Header,
                Description = "Authorization format : Bearer {token}",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement() {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "Bearer",
                            Name = "Bearer",
                            In = ParameterLocation.Header,

                        },
                        new List<string>()
                    }
     });
           // c.SwaggerDoc("v1", new OpenApiInfo().AddOpenApiInfo());
        }).AddTransient<IConfigureOptions<SwaggerGenOptions>,ConfigureSwaggerGenOptions>();              
      

            AddApiVersioningHelper.AddApiVersioning(services);          

            return services;
        }


    }

 

}
