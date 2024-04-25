using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IvoryPayAssessment.Application.Interfacses.UserAccounts
{
    public interface IAccountLogoutService
    {
        Task<ServerResponse<bool>> LogOut();      
        Task ClearsSession();
        //Task<ServerResponse<bool>> CheckLoginCount(string email);
    }
}
