using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvoryPayAssessment.Domain.Entities
{
    public class BaseObject
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public long Id { get; set; }
        public DateTime DateCreated { get; set; }  
        public string? CreatedBy {  get; set; }    
        public bool IsActive { get; set; } 
        public bool IsDeleted { get; set; }
     
    }
}
