using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvoryPayAssessment.Domain.Entities
{
    public class Session : BaseObject
    {

        public required string Token { get; set; }
        public required string UserId { get; set; }
    }
}
