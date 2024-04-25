using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvoryPayAssessment.Application.Extensions
{
    public static class OpenAPIInfoExtension
    {

        public static OpenApiInfo AddOpenApiInfo(this OpenApiInfo info)
        {



            info.Title = SwaggerOptionsHelper.Title();
            info.Version = string.IsNullOrWhiteSpace(SwaggerOptionsHelper.Version()) ? null : SwaggerOptionsHelper.Version();
            info.Description = SwaggerOptionsHelper.Description();
            info.TermsOfService = string.IsNullOrWhiteSpace(SwaggerOptionsHelper.TermsUrl()) ? null : new Uri(SwaggerOptionsHelper.TermsUrl());
            info.Contact = new OpenApiContact
            {
                Name = "Contact",
                Url = string.IsNullOrWhiteSpace(SwaggerOptionsHelper.ContactUrl()) ? null : new Uri(SwaggerOptionsHelper.ContactUrl())
            };
            info.License = new OpenApiLicense
            {
                Name = "License",
                Url = string.IsNullOrWhiteSpace(SwaggerOptionsHelper.LicenseUrl()) ? null : new Uri(SwaggerOptionsHelper.LicenseUrl())
            };          


            
            return info;
        }
    }
}
