
 

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var myAllowSpecificOrigins = "GhanaDrive";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
                      });
});
builder.Services.AddControllers(options =>
{
    options.InputFormatters.Insert(0, new BinaryInputFormatter());

});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();



#region  Add services to the container.

builder.Services.PushService(builder.Configuration, builder.Environment);
builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddRedisConfiguration(builder.Configuration);


ConfigureLogs();
builder.Host.UseSerilog();



builder.Services.AddControllers(options =>
{
    //Add global filters
    //options.Filters.Add(new ApiExceptionFilter());

}).AddMvcOptions(options =>
{
    options.Filters.Add(
    new ResponseCacheAttribute
    {
        NoStore = true,
        Location = ResponseCacheLocation.None

    });
    options.Filters.Add<LanguageFilter>();
    options.Filters.Add<SessionFilter>();
 
}
).AddNewtonsoftJson()
   .AddJsonOptions(options =>
   {
       options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
   });



#endregion


var app = builder.Build();

var logFactory = app.Services.GetRequiredService<ILoggerFactory>();


var config = app.Services.GetService<IConfiguration>() ?? throw new ArgumentNullException("config");
var env = app.Services.GetService<IHostEnvironment>() ?? throw new ArgumentNullException("env");
SwaggerOptionsHelper.SwaggerOptionsHelperConfifure(config);

var connectionString = app.Configuration.GetConnectionString("DefaultConnection");
var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
using (var scope = scopeFactory.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    await CreateDefaultRecords(userManager, roleManager, app.Configuration);
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsProduction() || app.Environment.IsStaging())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.UseSwagger(o => o.RouteTemplate = "swagger/{documentName}/swagger.json");
    app.MapSwagger("swagger/{documentName}/swagger.json", p => { p.SerializeAsV2 = true; }).RequireAuthorization();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseMiddleware<RateLimitingMiddleware>();

app.UseHttpsRedirection();
app.UseAuthorization();
app.UseCors(myAllowSpecificOrigins);
app.MapControllers();
app.Run();


#region Elastic Configuration

void ConfigureLogs()
{


    //get configuration files
    IConfigurationRoot configuration = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", false, true)
        .Build();
    //get the environment which the app is running on
    var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

    if (env is null)
    {
        env = configuration.GetValue<string>("Env:Environment");
    }
    //Logger
    Log.Logger = new LoggerConfiguration()
          .Enrich.FromLogContext()
          .Enrich.WithExceptionDetails()//add exception details
          .WriteTo.Debug()
          .WriteTo.Console()
          .WriteTo.File($"{builder.Environment.ContentRootPath}{Path.DirectorySeparatorChar}ServiceLogs/ghanadrive-", rollingInterval: RollingInterval.Day)

          .CreateLogger();




}




#endregion



#region Create   Roles

async Task CreateDefaultRecords(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IConfiguration conf)
{
    await CreateRole(roleManager, conf);
    await CreateUsers(userManager, conf);

}


async Task CreateRole(RoleManager<ApplicationRole> roleManager, IConfiguration conf)
{

    var roles = EnumHelper.GetRoles();
    foreach (var rol in roles)
    {
        var rol_ = await roleManager.FindByNameAsync(rol.ToString());
        if (rol_ == null)
        {
            var role = new ApplicationRole
            {
                Name = rol.ToString(),
                IsActive = true,
                Id = Guid.NewGuid().ToString("N"),

            };
            role.CreatedBy = null;
            role.DateCreated = DateTime.Now;
            role.IsActive = true;
            await roleManager.CreateAsync(role);
        }


    }


}


async Task CreateUsers(UserManager<ApplicationUser> userManager, IConfiguration conf)
{

    var dev = conf.GetValue<string>("DefaultLogins:Dev");
    var password = conf.GetValue<string>("DefaultLogins:Password");
    var devPhone = conf.GetValue<string>("DefaultLogins:DevPhone");


    var userExists = await userManager.FindByEmailAsync(dev);
    if (userExists == null)
    {
        var user = new ApplicationUser
        {
            Id = Guid.NewGuid().ToString("N"),
            UserName = dev,
            Email = dev,
            NormalizedEmail = dev.ToUpper(),
            NormalizedUserName = dev.ToUpper(),
            DateCreated = DateTime.Now,
            EmailConfirmed = true,
            PhoneNumber = devPhone,
            DefaultRole = DefaultRole.Dev.ToString(),
            FirstName = "Dev",
            LastName = "I",
            IsActive = true,
            PhoneNumberConfirmed = true,
            DateofBirth = DateTime.Today.AddYears(34),
            FullName = "Developer"


        };

        var identityresult = userManager.CreateAsync(user, password)?.GetAwaiter().GetResult();
        if (identityresult != null && identityresult.Succeeded)
        {
            userManager.AddToRoleAsync(user, DefaultRole.Dev.ToString())?.GetAwaiter().GetResult();
        }

    }

}
#endregion
