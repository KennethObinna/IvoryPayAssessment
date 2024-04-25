using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvoryPayAssessment.Domain.Entities
{
    public class ApplicationRoleClaim : IdentityRoleClaim<string>
    {
        public DateTime DateCreated { get; set; }     
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
