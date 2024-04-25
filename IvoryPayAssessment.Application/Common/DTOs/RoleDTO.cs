using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvoryPayAssessment.Application.Common.DTOs
{
    public class RoleDTO
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public bool IsValid(out ValidationResponse source, IMessageProvider messageProvider, string language)
        {

            if (string.IsNullOrWhiteSpace(Name))
            {
                var message = $"{nameof(Name)} {messageProvider.GetMessage(ResponseCodes.DATA_IS_REQUIRED, language)}";
                source = new ValidationResponse
                {
                    Message = message,
                    Code = ResponseCodes.DATA_IS_REQUIRED
                };

                return false;
            }
            source = new ValidationResponse();
            return true;
        }


        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }

    }

    public class RoleModelView
    {
        public List<RolesView> RolesViewModel { get; set; }


    }
    public class RolesView
    {
        public string RoleName { get; set; }
        public string Id { get; set; }

    }
    public class RoleDTOV2
    {
        public string RoleName { get; set; }

        public bool IsValid(out ValidationResponse source, IMessageProvider messageProvider, string language)
        {

            if (string.IsNullOrWhiteSpace(RoleName))
            {
                var message = $"{nameof(RoleName)} {messageProvider.GetMessage(ResponseCodes.DATA_IS_REQUIRED, language)}";
                source = new ValidationResponse
                {
                    Message = message,
                    Code = ResponseCodes.DATA_IS_REQUIRED
                };

                return false;
            }
            source = new ValidationResponse();
            return true;
        }
    }
}
