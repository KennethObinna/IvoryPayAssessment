using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvoryPayAssessment.Application.Common.Models
{
    public class SystemSettings
    {
        public string LanguagePackFolderName { get; set; }
        public bool IsTest { get; set; }
        public int SamePasswordChangeCount { get; set; }
        public string OtherFiles { get; set; }
        public string VideoLocalPath { get; set; }
        public string VideoWebFileURL { get; set; }
        public string OtherWebFileURL { get; set; }
        public int AppIdLength { get; set; }
        public int ClientIdLength { get; set; }
        public string VatebraPhone { get; set; }
        public string BaseUrl { get; set; }
        public string EmailVerificationUrl { get; set; }
        public string ResetPasswordUrl { get; set; }
      
        public string GhanaDriveWebUrl { get; set; }
        public int CustomerActiveMonths { get; set; }
        public int PagenationSize { get; set; }
    }
 
}
