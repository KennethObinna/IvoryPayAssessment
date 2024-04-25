using System.Threading.Tasks;

namespace IvoryPayAssessment.Application.Interfacses.UserAccounts
{
    public interface IUserService
    {
        Task<ServerResponse<bool>> IsUserExists(string userEmail);   
        Task<ServerResponse<UserDto>> CreateUser(RegisterViewModel model);  
 
        Task<ServerResponse<List<UserDto>>> GetAllUsers(); 
      

    }
}