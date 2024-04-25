 
using Microsoft.AspNetCore.Mvc.Versioning;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvoryPayAssessment.Application.Common.Helpers
{
    public class AddApiVersioningHelper
    {
        public static void AddApiVersioning(IServiceCollection services)
        {
           
            services.AddApiVersioning(
       options =>
       {
           options.AssumeDefaultVersionWhenUnspecified = true;
           options.ReportApiVersions = true;
       
           options.DefaultApiVersion = new ApiVersion(1,0);
       });
            services.AddVersionedApiExplorer(
                options =>
                {
                    options.GroupNameFormat = "'v'VVV";               
                    options.SubstituteApiVersionInUrl = true;
                
                });
        }
    }
}
