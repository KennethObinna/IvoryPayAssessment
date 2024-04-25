using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvoryPayAssessment.Domain.Entities
{
    public class ApplicationUserLogin : IdentityUserLogin<string>
    {
 
        public DateTime DateCreated { get; set; } 
     
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

    }
}
