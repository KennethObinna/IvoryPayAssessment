using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvoryPayAssessment.Application.Helpers
{
    public class EnumHelper
    {

        public static List<string> GetRoles()
        {
            var roles = Enum.GetNames(typeof(DefaultRole)).ToList();
            return roles;
        }
      
    }
}
