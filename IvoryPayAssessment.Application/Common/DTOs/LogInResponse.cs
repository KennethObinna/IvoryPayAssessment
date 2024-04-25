using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvoryPayAssessment.Application.Common.DTOs
{
    public class LogInResponse
    {
        public UserDto User { get; set; }
        public Tokens Token { get; set; }
    }
}
