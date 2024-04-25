using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvoryPayAssessment.Application.Common.DTOs
{
    public class UserRefreshTokenDto
    {

        public long Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string RefreshToken { get; set; }
        public bool IsActive { get; set; } = true;
        public string ClientId { get; set; }
    }
}
