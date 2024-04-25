
using IvoryPayAssessment.Application.Common.Helpers;
 
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Caching.Memory;
using Mapster;

namespace IvoryPayAssessment.Application.Implementations.UserAccount
{
    public class AccountLogInService : ResponseBaseService, IAccountLogInService
    {
        private readonly IMessageProvider _messageProvider;
        private readonly IHttpContextAccessor _httpContext;

        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<AccountLogoutService> _logger;
        private readonly ISessionsService _sessionsService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITokenService _tokenService;
     
        private readonly IAppDbContext _context;
     
        private readonly string _language;
     
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IDbContextTransaction _trans;
        public AccountLogInService(
            IMessageProvider messageProvider,
            IHttpContextAccessor httpContext,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            ITokenService tokenService,
            ILogger<AccountLogoutService> logger,
            ISessionsService sessionsService, IAppDbContext context, IConfiguration config, SystemSettings systemSetting) : base(messageProvider)
        {
            _messageProvider = messageProvider ?? throw new ArgumentNullException(nameof(messageProvider));
            _httpContext = httpContext ?? throw new ArgumentNullException(nameof(httpContext));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _sessionsService = sessionsService ?? throw new ArgumentNullException(nameof(sessionsService));             
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _language = _httpContext.HttpContext.Request.Headers[ResponseCodes.LANGUAGE];
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));         
            _trans = _context.Begin();
        }

        public async Task<ServerResponse<LogInResponse>> LogIn(string email, string password)
        {
            var response = new ServerResponse<LogInResponse>();

            var user = await _context.Users.FirstOrDefaultAsync(p => p.UserName == email);//  _userManager.FindByNameAsync(email);

            if (user == null)
            {
                SetError(response, ResponseCodes.INVALID_EMAIL_OR_PASSWORD, _language);
                return response;
            }
             
            var signInResult = await _signInManager.PasswordSignInAsync(user, password, false, false);

            if (signInResult != null && signInResult.Succeeded)
            {
                UserDto userDto = user.Adapt<UserDto>();
              
                userDto.UserId = user.Id;                 
        
              
                var tokens = await _tokenService.GenerateToken(userDto);

                var sessionRequest = new SessionDTO
                {
                    DateCreated = DateTime.Now,
                    Token = tokens?.Data?.Token,
                    UserId = user.Id,
                };

                var sessionResponse = await _sessionsService.CreateSessionAsync(sessionRequest, _language);
                if (sessionResponse != null && sessionResponse.IsSuccessful)
                {
                  
               await _userManager.UpdateAsync(user);

                   var data = new LogInResponse
                    {
                        User = userDto,                       
                        Token = tokens?.Data,
                       
                    };
                    SetSuccess(response, data, ResponseCodes.SUCCESS, _language);
                    await _trans.CommitAsync();
                }
                else
                {
                    response.IsSuccessful = false;
                    response.Data = null;
                    response.Error = sessionResponse.Error;
                    await _trans.RollbackAsync();
                    return response;
                }

            }
            else
            {
                response.IsSuccessful = false;
                response.Data = null;
                response.Error = new ErrorResponse
                {
                    responseCode = ResponseCodes.UNAUTHORIZED,
                    responseDescription = _messageProvider.GetMessage(ResponseCodes.INVALID_EMAIL_OR_PASSWORD, _language)
                };
            }

            return response;
        }
         
    }
}
