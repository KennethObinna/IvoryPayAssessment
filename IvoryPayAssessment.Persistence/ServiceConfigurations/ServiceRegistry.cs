


using IvoryPayAssessment.Application.Implementations.ProductCategories;
using IvoryPayAssessment.Application.Implementations.Products;
using IvoryPayAssessment.Application.Implementations.Restuarants;
using IvoryPayAssessment.Application.Interfacses.ProductCategories;
using IvoryPayAssessment.Application.Interfacses.Products;
using IvoryPayAssessment.Application.Interfacses.Restuarants;

namespace IvoryPayAssessment.Persistence.ServiceConfigurations
{
    public static class ServiceRegistry
    {

        public static IServiceCollection PushService(this IServiceCollection services, IConfiguration conf, IWebHostEnvironment hostingEnvironment)
        {
            //Add services here
 
            #region Identity Options
            services.Configure<IdentityOptions>(options =>
              {
                
                
                  options.Lockout.AllowedForNewUsers = true;
                  options.Password.RequireDigit = false;
                  options.Password.RequireLowercase = false;
                  options.Password.RequireNonAlphanumeric = false;
                  options.Password.RequireUppercase = false;
                  options.Password.RequiredLength = 6;
                  options.Password.RequiredUniqueChars = 1;
                  options.SignIn.RequireConfirmedEmail = false;
                  options.SignIn.RequireConfirmedPhoneNumber = false;
              });
            #endregion

            #region Mapster 
            TypeAdapterConfig.GlobalSettings.Default
           .NameMatchingStrategy(NameMatchingStrategy.IgnoreCase)
           .IgnoreNullValues(true)
           .AddDestinationTransform((string x) => x.Trim())
           .AddDestinationTransform((string x) => x ?? "")
           .AddDestinationTransform((decimal? x) => x ?? 0)
           .AddDestinationTransform((double? x) => x ?? 0)
           .AddDestinationTransform(DestinationTransform.EmptyCollectionIfNull);
            services.RegisterMapsterConfiguration();

            #endregion

            #region Other services
            string connectionString = string.Empty;
            var env = conf.GetValue<string>("Env:Environment");

            connectionString = conf["ConnectionStrings:IvoryPayAssessmentConnection"];

            services.AddDbContext<IAppDbContext, IvoryPayAssessmentDbContext>(options => options.UseSqlServer(connectionString));
            services.AddIdentity<ApplicationUser, ApplicationRole>().AddEntityFrameworkStores<IvoryPayAssessmentDbContext>()
                .AddDefaultTokenProviders();
            services.AddScoped<UserManager<ApplicationUser>>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<ISessionsService, SessionsService>();
            services.AddScoped<IAccountLogInService, AccountLogInService>();
            services.AddScoped<IAccountLogoutService, AccountLogoutService>();
            services.AddHttpContextAccessor();      
             
            string? httpClientName = conf["SystemSettings:HttpClientName"];
            string baseUrl = conf["SystemSettings:WebBaseUrl"];
            services.AddHttpClient(httpClientName ?? "");
            services.AddScoped<IAppDbContext, IvoryPayAssessmentDbContext>();
            services.AddScoped<ILanguageConfigurationProvider, LanguageConfigurationProvider>();
            services.AddScoped<IMessageProvider, MessageProvider>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<LanguagePackagePathHelper>();
            services.AddScoped<IAccountLogoutService, AccountLogoutService>();
            services.AddTransient<IAccountLogInService, AccountLogInService>();
            services.AddTransient<IOTPService, OTPService>();
            services.AddTransient<IRestaurantService, RestaurantService>();
            services.AddTransient<IProductCategoryService, ProductCategoryService>();
            services.AddTransient<IProductService, ProductService>();
            services.AddScoped<RestuarantHelper>();
            #endregion

            #region Static Files
            services.Configure<LanguageSettings>(options => conf.GetSection("LanguageSettings").Bind(options));
            services.Configure<AllowableActionmethods>(options => conf.GetSection("AllowableActionmethods").Bind(options));
            services.AddSingleton(conf.GetSection("SystemSettings").Get<SystemSettings>());
            services.AddSingleton(opt => conf.GetSection("LanguageSettings").Get<LanguageSettings>());
            #endregion

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

           
            return services;
        }
    }
}
