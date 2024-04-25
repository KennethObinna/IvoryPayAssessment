


using System.Security.Claims;

namespace IvoryPayAssessment.Application.Interfacses
{
    public interface ITokenService
    {


        Task<ServerResponse<Tokens>> GenerateToken(UserDto user);
        Task<ServerResponse<Tokens>> GenerateRefreshToken(UserDto user);
		ClaimsPrincipal GetPrincipalFromExpiredToken(string token);

		Task<bool> AddUserRefreshTokens(UserRefreshTokenDto request);
		Task DeleteUserRefreshTokens(string username, string refreshToken);
		Task<UserRefreshTokenDto> GetSavedRefreshTokens(string username, string refreshToken);
		TokenConvert GetToken(string token="");
        public bool IsAuthenticated { get; set; }
        // Task<ServerResponse<Tokens>> GenerateRefreshTokenNew( );

    }
}
