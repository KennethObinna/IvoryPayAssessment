


using IvoryPayAssessment.Application.Common.DTOs;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;
using System.Linq;

namespace IvoryPayAssessment.Application.Implementations.UserAccounts
{
    public class UserService : ResponseBaseService, IUserService

    {
        private readonly ILogger<UserService> _logger;
        private readonly IMessageProvider _messageProvider;
        private readonly IHttpContextAccessor _httpContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IConfiguration _config;
        private readonly IAppDbContext _context;
        private readonly IDbContextTransaction _trans;
        private readonly string _language;
        private readonly ITokenService _tokenService;
        private readonly IOTPService _oTPService;
        public UserService(IMessageProvider messageProvider,
            IHttpContextAccessor httpContext,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IConfiguration config,

            IAppDbContext context,
            IHostEnvironment hostEnvironment,
            RoleManager<ApplicationRole> roleManager,


            ITokenService tokenService, ILoggerFactory log, SystemSettings systemSettings, IOTPService oTPService) : base(messageProvider)
        {
            _messageProvider = messageProvider ?? throw new ArgumentNullException(nameof(messageProvider));
            _httpContext = httpContext ?? throw new ArgumentNullException(nameof(httpContext));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _config = config ?? throw new ArgumentNullException(nameof(config)); ;
            _context = context ?? throw new ArgumentNullException(nameof(_context));
            _trans = _context.Begin();
            _language = _httpContext.HttpContext.Request.Headers[ResponseCodes.LANGUAGE];
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            _tokenService = tokenService ?? throw new ArgumentNullException("tokenService");
            _logger = log.CreateLogger<UserService>();

            _oTPService = oTPService ?? throw new ArgumentNullException(nameof(oTPService));
        }


        public async Task<ServerResponse<UserDto>> CreateUser(RegisterViewModel request)
        {
            var response = new ServerResponse<UserDto>();


            if (!request.IsValid(out ValidationResponse error, _messageProvider, _httpContext))
            {
                return SetError(response, error.Code, error.Message, _language);
            }





            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser != null)
            {
                SetError(response, ResponseCodes.RECORD_EXISTS, _language);
                return response;
            }

          
          
           
            UserDto userDto = new UserDto();

            var newUser = request.Adapt<ApplicationUser>();
            newUser.DateCreated = DateTime.Now;
            newUser.UserName = request.Email;
            newUser.IsActive = true;
            newUser.EmailConfirmed = false;
            newUser.FullName = $"{request.FirstName} {request.LastName}";
            newUser.Id = Guid.NewGuid().ToString("N");

            newUser.DefaultRole = request.UserType.ToString();



 
          
            var result = await _userManager.CreateAsync(newUser, request.Password);
            if (!result.Succeeded)


            {

                await _trans.RollbackAsync();
                SetIdentityError(response, ResponseCodes.IDENTITY_ERROR, String.Join(",", result.Errors.Select(p => p.Description)), _language);
                return response;
            }
            else
            {


 
                userDto = newUser.Adapt<UserDto>();
                await _trans.CommitAsync();

                SetSuccess(response, userDto, ResponseCodes.SUCCESS, _language);

            }


            return response;

        }

        public async Task<ServerResponse<List<UserDto>>> GetAllUsers()
        {

            var response = new ServerResponse<List<UserDto>>();
            var token = _tokenService.GetToken();
            if (token == null)
            {
                SetError(response, ResponseCodes.INVALID_TOKEN, _language); return response;
            }
            var user = token.User;
            if (user is null)
            {
                SetError(response, ResponseCodes.INVALID_USER, _language); return response;
            }


            var usersWithRoles = await _context.Users
                .Join(_context.UserRoles, user => user.Id, userRole => userRole.UserId, (user, userRole) => new { user, userRole })
                .Join(_context.Roles, ur => ur.userRole.RoleId, role => role.Id, (ur, role) => new { ur.user, ur.userRole.RoleId, role.Name })
                .GroupBy(x => x.user.Id)

                .Select(group => new UserDto
                {
                    UserName = group.First().user.UserName,
                    Email = group.First().user.Email,
                    PhoneNumber = group.First().user.PhoneNumber,
                    UserId = group.First().user.Id,
                    FirstName = group.First().user.FirstName,
                    LastName = group.First().user.LastName,
                    IsActive = group.First().user.IsActive,
                    DateCreated = group.FirstOrDefault().user.DateCreated,
                    DateofBirth = group.FirstOrDefault().user.DateofBirth,

                })
                .ToListAsync();
            SetSuccess(response, usersWithRoles, ResponseCodes.SUCCESS, _language);

            return response;
        }
        public async Task<ServerResponse<bool>> IsUserExists(string userEmail)
        {

            var response = new ServerResponse<bool>();
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user != null && user.IsActive)
            {
                SetSuccess(response, true, ResponseCodes.SUCCESS, _language);
            }
            else
            {
                SetError(response, ResponseCodes.NOT_FOUND, _language);
            }

            return response;
        }




    }
}
