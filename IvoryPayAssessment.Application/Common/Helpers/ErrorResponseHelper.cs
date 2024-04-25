using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvoryPayAssessment.Application.Common.Helpers
{
    public class ErrorResponseHelper
    {
        private readonly IMessageProvider _messageProvider;

        public ErrorResponseHelper(IMessageProvider messageProvider )
        {
            _messageProvider = messageProvider;
        }
    }
}
