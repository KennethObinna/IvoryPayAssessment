using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvoryPayAssessment.Domain.Entities
{
    public class ApplicationUserRole : IdentityUserRole<string>
    {


      
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

    }
}
