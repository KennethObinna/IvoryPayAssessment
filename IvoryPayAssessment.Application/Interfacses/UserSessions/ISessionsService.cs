using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvoryPayAssessment.Application.Interfacses.UserSessions
{
    public interface ISessionsService
    {
        Task<ServerResponse<bool>> CreateSessionAsync(SessionDTO request, string language, bool isInternal = true);
        Task<ServerResponse<bool>> DeleteSessionAsync(string userId, string language);
        Task<ServerResponse<bool>> UpdateSessionAsync(UpdateSessionDTO request, string language);
      Task  RemoveCookies();
        Task<bool> IsSessionValid();
    }
}
