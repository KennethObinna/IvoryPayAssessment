namespace IvoryPayAssessment.Application.Interfacses.UserAccounts
{
    public interface IAccountLogInService
    {
        Task<ServerResponse<LogInResponse>> LogIn(string email, string password);
     


    }
}