using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvoryPayAssessment.Domain.Entities
{
    public class ApplicationRole:IdentityRole<string>
    {
  
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdate { get; set; }
        public string? CreatedBy { get; set; }     
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
