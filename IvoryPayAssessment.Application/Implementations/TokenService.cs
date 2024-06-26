﻿


using Microsoft.AspNetCore.Authentication;
using Microsoft.Data.SqlClient;
using StackExchange.Redis;

namespace IvoryPayAssessment.Application.Implementations
{
    public class TokenService : ResponseBaseService, ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IAppDbContext _context;
        private readonly ILogger<TokenService> _log;
        private readonly IHttpContextAccessor _httpContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly string _token;
        private readonly string _language;



        public TokenService(IConfiguration iconfiguration, IAppDbContext context, ILogger<TokenService> log, IHttpContextAccessor httpContext, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IMessageProvider message):base(message)
        {
            _configuration = iconfiguration ?? throw new ArgumentNullException(nameof(_configuration));
            _context = context ?? throw new ArgumentNullException(nameof(context)); ;
            _log = log;
            _httpContext = httpContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _token = _httpContext.HttpContext.GetTokenAsync("access_token")?.GetAwaiter().GetResult();
            _language = _httpContext.HttpContext.Request.Headers[ResponseCodes.LANGUAGE];
        }
        public async Task<ServerResponse<Tokens>> GenerateToken(UserDto user)
        {
            return await GenerateJWTTokens(user);
        }

        public async Task<ServerResponse<Tokens>> GenerateRefreshToken(UserDto user)
        {
            return await GenerateJWTTokens(user);
        }

        public async Task<ServerResponse<Tokens>> GenerateJWTTokens(UserDto user)
        {
            var response=new ServerResponse<Tokens>();
            try
            {
                var data = user;
                if (data is null)
                {
                    return SetError(response, ResponseCodes.INVALID_USER, _language);
                }

                if (string.IsNullOrWhiteSpace(user.Email))
                {
                    return SetError(response, ResponseCodes.INVALID_OBJECT, _language);
                }
                
               
                // Else we generate JSON Web Token
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = _configuration["ApplicationSettings:SecurityKey"];
                var tokenKey = Encoding.UTF8.GetBytes(key);
                //var expirationMinutes = _configuration["jwt:TimeoutMinutes"];
                var expirationMinutes = 10;
                var subject = new ClaimsIdentity(new Claim[]
                      {
                             new Claim(ClaimTypes.Name, data.UserName),
                             new Claim(ClaimTypes.Email, data.Email),                      
                             new Claim ("User", JsonConvert.SerializeObject(user)),
                             new Claim("UserId",Convert.ToString(user.UserId))
                      });
                var signature = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = subject,
                    Expires = DateTime.UtcNow.AddMinutes(Convert.ToInt32(expirationMinutes)),
                    SigningCredentials = signature
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var refreshToken = GenerateRefreshToken();
                await Task.CompletedTask;
                var result= new Tokens { Token = tokenHandler.WriteToken(token), RefreshToken = refreshToken };
                SetSuccess(response, result, ResponseCodes.SUCCESS, _language);
                return response;
               
            }
            catch (Exception ex)
            {
                //Add Logging

            }
            await Task.CompletedTask;
            return null;

        }
        public bool IsAuthenticated { get; set; } 
        public TokenConvert GetToken(string tok = "")
        {


            var claimsIdentity = _httpContext.HttpContext.User.Identity as ClaimsIdentity;
            if (claimsIdentity.IsAuthenticated)
            {
                   if (claimsIdentity != null)
            {
                IEnumerable<Claim> claims = claimsIdentity.Claims;
                if (claims != null && claims.Count() > 0)
                {
                    string userDetails = claimsIdentity.FindFirst(p => p.Type == "User")?.Value;
                    string name = claimsIdentity.FindFirst(p => p.Type == "Name")?.Value;
                    string email = claimsIdentity.FindFirst(p => p.Type == "Email")?.Value;
                    string userId = claimsIdentity.FindFirst(p => p.Type == "UserId")?.Value??"";
                    string userPermission = claimsIdentity.FindFirst(p => p.Type == "UserPermissions")?.Value;

                    var user = claimsIdentity.FindFirst("User")?.Value;

                      var userDetailsDeserialize = !string.IsNullOrWhiteSpace(userDetails) ? JsonConvert.DeserializeObject<UserDto>(userDetails) : new UserDto();
                        IsAuthenticated = true;
                        return new TokenConvert
                    {
                        Name = name,
                        Email = email,
                        UserId = userId,                       
                        User = userDetailsDeserialize

                    };
                }
                else
                {
                    var token = !string.IsNullOrWhiteSpace(tok) ? tok : _token;
                    if (string.IsNullOrWhiteSpace(token))
                    {
                        return null;
                    }
                    var handler = new JwtSecurityTokenHandler();
                    var jwtSecurityToken = handler.ReadJwtToken(token);

                    if (jwtSecurityToken != null)
                    {
                        var claimsIdentitys = jwtSecurityToken.Claims;
                        // or
                        string userDetails = claimsIdentitys.FirstOrDefault(p => p.Type == "User")?.Value;
                        string name = claimsIdentitys.FirstOrDefault(p => p.Type == "Name")?.Value;
                        string email = claimsIdentitys.FirstOrDefault(p => p.Type == "Email")?.Value;
                        string userId = claimsIdentitys.FirstOrDefault(p => p.Type == "UserId")?.Value ?? "";
                        string userPermission = claimsIdentitys.FirstOrDefault(p => p.Type == "UserPermissions")?.Value;

                        var user = claimsIdentity.FindFirst("User")?.Value;

                           var userDetailsDeserialize = !string.IsNullOrWhiteSpace(userDetails) ? JsonConvert.DeserializeObject<UserDto>(userDetails) : new UserDto();
                            IsAuthenticated = true;
                            return new TokenConvert
                        {
                            Name = name,
                            Email = email,
                            UserId = userId,                           
                            User = userDetailsDeserialize
                        };
                    }

                }
            }
              
            }
            else
            {
                IsAuthenticated = false;
            }
         

            return new TokenConvert();
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var Key = Encoding.UTF8.GetBytes(_configuration["ApplicationSettings:SecurityKey"]);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = _configuration.GetValue<bool>("Jwt:ValidateSigningKey"),
                IssuerSigningKey = new SymmetricSecurityKey(Key),
                ValidateIssuer = _configuration.GetValue<bool>("Jwt:ValidateIssuer"),
                ValidIssuer = _configuration.GetValue<string>("Jwt:Issuer"),
                ValidateAudience = _configuration.GetValue<bool>("Jwt:ValidateAudience"),
                ValidAudience = _configuration.GetValue<string>("Jwt:Audience"),
                ValidateLifetime = _configuration.GetValue<bool>("Jwt:ValidateLifeTime"),
                ClockSkew = TimeSpan.FromMinutes(_configuration.GetValue<int>("Jwt:DateToleranceMinutes"))
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            JwtSecurityToken jwtSecurityToken = securityToken as JwtSecurityToken ?? throw new ArgumentNullException(nameof(securityToken));
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }


            return principal;
        }

        public async Task<bool> AddUserRefreshTokens(UserRefreshTokenDto request)
        {
            var data = request.Adapt<UserRefreshToken>();
            await _context.UserRefreshTokens.AddAsync(data);
            var saved = await _context.SaveChangesAsync();
            return saved > 0;

        }
        public async Task DeleteUserRefreshTokens(string username, string refreshToken)
        {
            var item = await _context.UserRefreshTokens.FirstOrDefaultAsync(x => x.UserName == username && x.RefreshToken == refreshToken);
            if (item != null)
            {
                _context.UserRefreshTokens.Remove(item);
            }
        }
        public async Task<UserRefreshTokenDto> GetSavedRefreshTokens(string username, string refreshToken)
        {
            var data = await _context.UserRefreshTokens.FirstOrDefaultAsync(x => x.UserName == username && x.RefreshToken.Equals(refreshToken) && x.IsActive.Equals(true));
            if (data != null) { return data.Adapt<UserRefreshTokenDto>(); }
            else { return new UserRefreshTokenDto(); }
        }
    }
}

