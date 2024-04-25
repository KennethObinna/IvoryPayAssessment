using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvoryPayAssessment.Application.Common.DTOs
{
    public class BaseObject
    {
       

        public long Id { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateUpdate { get; set; }
        public long CreatedBy {  get; set; }
        public long? UpdatedBy { get; set;}
        public bool IsActive {  get; set; }
        public bool IsDeleted { get; set; }
    }
}
