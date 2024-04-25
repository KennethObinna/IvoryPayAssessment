using System.Net;

namespace IvoryPayAssessment.Application.Implementations.UserSessions
{
    public class SessionsService : ResponseBaseService, ISessionsService
    {
        private readonly IAppDbContext _context;
        private readonly ILogger<SessionsService> _logger;
        private readonly IMessageProvider _messageProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IDbContextTransaction _trans;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenService _tokenService;
        public SessionsService(IAppDbContext context, ILogger<SessionsService> logger, IMessageProvider messageProvider, IHttpContextAccessor httpContextAccessor, SignInManager<ApplicationUser> signInManager, ITokenService tokenService) : base(messageProvider)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _messageProvider = messageProvider ?? throw new ArgumentNullException(nameof(messageProvider)); ;
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
            _trans = _context.Begin();
            _signInManager = signInManager;
            _tokenService = tokenService;
        }

        public async Task<ServerResponse<bool>> CreateSessionAsync(SessionDTO request, string language, bool isInternal = true)
        {
            var response = new ServerResponse<bool>();
            if (!request.IsValid(out ValidationResponse source, _messageProvider, _httpContextAccessor))
            {
                response.Error = new ErrorResponse
                {
                    responseCode = source.Code,
                    responseDescription = source.Message
                };
                return response;
            }

            var dataMapped = request.Adapt<Session>();
            if (dataMapped is null)
            {
                response.Error = new ErrorResponse
                {
                    responseCode = ResponseCodes.INVALID_OBJECT_MAPPING,
                    responseDescription = _messageProvider.GetMessage(ResponseCodes.INVALID_OBJECT_MAPPING, language)
                };
                return response;
            }

            var sessionLogged = await _context.Sessions.FirstOrDefaultAsync(p => p.UserId.Equals(request.UserId));

            if (sessionLogged != null)
            {
                var user = await _context.Users.FirstOrDefaultAsync(p => p.Id == request.UserId);
                if (user != null)
                {

                    _context.Users.Update(user);
                }
                await _signInManager.SignOutAsync();
                _context.Sessions.Remove(sessionLogged);


            }

            dataMapped.CreatedBy = request.UserId;
            await _context.Sessions.AddAsync(dataMapped);
            int save = await _context.SaveChangesAsync();
            if (save > 0)
            {
                SetSuccess(response, true, ResponseCodes.SUCCESS, language);
            }
            else
            {

                SetError(response, ResponseCodes.REQUEST_NOT_SUCCESSFUL, language);
            }
            return response;
        }

        public async Task<ServerResponse<bool>> DeleteSessionAsync(string userId, string language)
        {
            var response = new ServerResponse<bool>();
            if (string.IsNullOrWhiteSpace(userId))
            {
                response.Error = new ErrorResponse
                {
                    responseCode = ResponseCodes.INVALID_PARAMETER,
                    responseDescription = _messageProvider.GetMessage(ResponseCodes.INVALID_PARAMETER, language)
                };
                return response;

            }

            var record = await _context.Sessions.FirstOrDefaultAsync(x => x.UserId.Equals(userId));
            if (record == null)
            {
                SetError(response, ResponseCodes.RECORD_NOT_FOUND, language);

                return response;

            }
            _context.Sessions.Remove(record);
            int save = await _context.SaveChangesAsync();
            if (save > 0)
            {
                await RemoveCookies();
                await _trans.CommitAsync();
                SetSuccess(response, true, ResponseCodes.REQUEST_NOT_SUCCESSFUL, language);
            }
            else
            {
                response.SuccessMessage = _messageProvider.GetMessage(ResponseCodes.REQUEST_NOT_SUCCESSFUL, language);
            }
            return response;
        }

        public async Task<ServerResponse<bool>> UpdateSessionAsync(UpdateSessionDTO request, string language)
        {
            var response = new ServerResponse<bool>();
            if (request.IsValid(out ValidationResponse source, _messageProvider, _httpContextAccessor))
            {
                response.Error = new ErrorResponse
                {
                    responseCode = source.Code,
                    responseDescription = source.Message
                };
                return response;
            }

            var record = await _context.Sessions.FirstOrDefaultAsync(x => x.UserId.Equals(request.UserId));
            if (record is null)
            {
                response.Error = new ErrorResponse
                {
                    responseCode = ResponseCodes.INVALID_OBJECT,
                    responseDescription = _messageProvider.GetMessage(ResponseCodes.INVALID_OBJECT, language)
                };
                return response;
            }

            record.Token = request.Token;
            _context.Sessions.Update(record);
            int save = await _context.SaveChangesAsync();
            if (save > 0)
            {
                await _trans.CommitAsync();
                response.Data = true; response.SuccessMessage = _messageProvider.GetMessage(ResponseCodes.SUCCESS, language);
            }
            else
            {
                response.SuccessMessage = _messageProvider.GetMessage(ResponseCodes.REQUEST_NOT_SUCCESSFUL, language);
            }
            return response;
        }


        public async Task RemoveCookies()
        {
            string cookie = string.Empty;

            var requestCookies = _httpContextAccessor.HttpContext.Request.Cookies;
            foreach (var cook in requestCookies)
                cookie = cook.Key;

            await Task.Run(() => { _httpContextAccessor.HttpContext.Response.Cookies.Delete(cookie); });

        }

        public async Task<bool> IsSessionValid()
        {
            var token = _tokenService.GetToken();

            if (token is null)
            {
                return false;
            }
            var session = await _context.Sessions.FirstOrDefaultAsync(p => p.UserId == token.UserId);
            if (session is null)
            {
                return false;
            }
            return true;
        }
    }
}
