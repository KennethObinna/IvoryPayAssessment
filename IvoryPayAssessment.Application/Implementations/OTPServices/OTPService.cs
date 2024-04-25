using IvoryPayAssessment.Application.Common.Constants.ErrorBuldles;



namespace IvoryPayAssessment.Application.Implementations.OTPServices
{
    public class OTPService : ResponseBaseService, IOTPService
    {
        private readonly IAppDbContext _context;
        private readonly IMessageProvider _messageProvider;
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly bool _isTest = false;
        private readonly IConfiguration _config;
        private string _otp = string.Empty;
        private readonly int _otpLength = 0;
        private readonly IDbContextTransaction _trans;
        private readonly IHttpContextAccessor _httpContext;
        private readonly string _language = string.Empty;
        private readonly int _otpValidTime = 0;
        private readonly ILogger _log;
        private readonly bool _isLogTest = false;
        public OTPService(IAppDbContext context, IMessageProvider messageProvider, IConfiguration config,
            UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContext,
          ILoggerFactory log, SystemSettings systemSettings) : base(messageProvider)
        {
            _context = context;
            _trans = _context.Begin();
            _messageProvider = messageProvider;

            _config = config;
            _isTest = _config.GetValue<bool>("SystemSettings:IsTest");
            _otp = _config.GetValue<string>("OTP:Code");
            _otpLength = _config.GetValue<int>("OTP:Len");
            _otpValidTime = _config.GetValue<int>("OTP:Duration");
            _userManager = userManager;
            _httpContext = httpContext;
            _language = _httpContext?.HttpContext?.Request?.Headers[ResponseCodes.LANGUAGE];
            _log = log.CreateLogger<OTPService>();
            _isLogTest = _config.GetValue<bool>("SystemSettings:IsLogTest");

        }

         public async Task<ServerResponse<string>> GenerateOTP(OTPRequest request, bool isInternal = true, bool resend = false)
        {
            var response = new ServerResponse<string>();
            string cipher = string.Empty;
            string otp = string.Empty;
            var key = string.Empty;

            ApplicationUser? user = new ApplicationUser();

            user = await _userManager.FindByEmailAsync(request.Email);


            if (user is null)
            {
                return SetError(response, ResponseCodes.INVALID_USER, _language);
            }
            if (_isTest)
            {

                cipher = bcrypt.EnhancedHashPassword(_otp, HashType.SHA512);

            }
            else
            {
                key = EncDecHelper.GenerateNumericKey(_otpLength);
                _otp = key;
                cipher = bcrypt.EnhancedHashPassword(_otp, HashType.SHA512);


            }
            if (_isLogTest)
            {
                _log.LogInformation("OTP ==>>{otp}", otp);
                _log.LogInformation("calling OTPChek method");
            }

            await OTPCheck(request, cipher, user);

            int save = await _context.SaveChangesAsync();
            if (save > 0)
            {
                if (!isInternal)
                {
                    await _trans.CommitAsync();
                }

                SetSuccess(response, _otp, ResponseCodes.SUCCESS, _language);




            }
            else
            {
                if (!isInternal)
                {
                    await _trans.RollbackAsync();
                }
                return SetErrorWithStatus(response, ResponseCodes.REQUEST_NOT_SUCCESSFUL, request.Email, _language);

            }


            return response;

        }
        public async Task<ServerResponse<bool>> ValidateOTP(ValidateOTPRequest request, bool isinternal = true)
        {
            OTPs? otp;
            var response = new ServerResponse<bool>();

            if (request.OTPType != OTPType.VerifyPhoneNumber)
            {
                otp = await _context.OTPs.FirstOrDefaultAsync(p => p.Email.Equals(request.Email) && p.OTPType.Equals(request.OTPType.ToString()));
            }
            else
            {
                otp = await _context.OTPs.FirstOrDefaultAsync(p => p.PhoneNumber.Equals(request.PhoneNumber) && p.OTPType.Equals(request.OTPType.ToString()));
            }

            if (otp is null)
            {

                return SetError(response, ResponseCodes.INVALID_OTP, _language);

            }
            var isActive = otp.IsActive == true;

            if (!isActive)
            {
                return SetError(response, ResponseCodes.INVALID_OTP, _language);
            }
            var hasExpired = HasExpired(otp.DateCreated);
            if (hasExpired)
            {
                return SetError(response, ResponseCodes.OTP_EXPIRED, _language);
            }
            bool isValid = bcrypt.EnhancedVerify(request.Code, otp.OTP, HashType.SHA512);
            if (!isValid)
            {

                return SetError(response, ResponseCodes.INVALID_OTP, _language);


            }
            else
            {
                otp.IsActive = false;
                _context.OTPs.Update(otp);
                int save = await _context.SaveChangesAsync();
                if (save > 0)
                {
                    response.IsSuccessful = true;
                    response.Data = true;
                    //if (isinternal)
                    //{
                    //    await _trans.CommitAsync();
                    //}

                    response.SuccessMessage = _messageProvider.GetMessage(ResponseCodes.OTP_VERIFIED, _language);

                }
                else
                {
                    //if (isinternal)
                    //{
                    //    await _trans.RollbackAsync();
                    //}


                    return SetError(response, ResponseCodes.REQUEST_NOT_SUCCESSFUL, _language);
                }


            }

            return response;
        }



        private async Task OTPCheck(OTPRequest request, string cipher, ApplicationUser? user)
        {
            string token = string.Empty;
            OTPs otp = new OTPs();
            OTPs getOtp = new OTPs();

            if (request.OTPType == OTPType.ResetPassword)
            {
                token = await _userManager.GeneratePasswordResetTokenAsync(user);
            }
            else if (request.OTPType == OTPType.VerifyEmail)
            {
                token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            }
            // else if(request.OTPType == OTPType.VerifyPhoneNumber)
            if (request.OTPType == OTPType.VerifyPhoneNumber)
            {
                otp = new OTPs { PhoneNumber = request.PhoneNumber, OTP = cipher, OTPType = request.OTPType.ToString(), Token = token, IsActive = true };
                getOtp = await _context.OTPs.FirstOrDefaultAsync(p => p.PhoneNumber.Equals(request.PhoneNumber) && p.OTPType.Equals(request.OTPType.ToString()));
            }
            else
            {
                otp = new OTPs { Email = request.Email, OTP = cipher, OTPType = request.OTPType.ToString(), Token = token, IsActive = true };
                getOtp = await _context.OTPs.FirstOrDefaultAsync(p => p.Email.Equals(request.Email) && p.OTPType.Equals(request.OTPType.ToString()));
            }

            if (getOtp is null)
            {
                otp.DateCreated = DateTime.Now;
                var entity = await _context.OTPs.AddAsync(otp);

            }
            else
            {
                getOtp.OTP = cipher;
                getOtp.Token = token;
                
                getOtp.DateCreated = DateTime.Now;
                getOtp.IsActive = true;
                getOtp.OTPType = request.OTPType.ToString();
                _context.OTPs.Update(getOtp);

            }
        }

        private bool HasExpired(DateTime inDate)
        {
            var dateNow = DateTime.Now;
            TimeSpan timeDifference = dateNow.Subtract(inDate);
            bool minutes = timeDifference.Minutes > _otpValidTime;
            _log.LogInformation("Times ==>> time now {t} our set time {tt}", JsonConvert.SerializeObject(timeDifference.Minutes), _otpValidTime);
            return minutes;
        }

    }
}
