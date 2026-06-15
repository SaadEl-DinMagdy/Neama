#region Configration Service
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Neama.Api.Errors;
using Neama.Api.Middlewares;
using Neama.Core;
using Neama.Core.Entities;
using Neama.Core.Repositories.Contract;
using Neama.Core.Services.Contract;
using Neama.Repository;
using Neama.Repository.Data;
using Neama.Service.AccountService;
using Neama.Service.AdminDashboard;
using Neama.Service.ApplicationToJoinService;
using Neama.Service.AttachmentService;
using Neama.Service.BasketService;
using Neama.Service.BranchDashboardService;
using Neama.Service.BranchService;
using Neama.Service.CategoryService;
using Neama.Service.CharityService;
using Neama.Service.FavoriteService;
using Neama.Service.ItemService;
using Neama.Service.LocationService;
using Neama.Service.MainSectionService;
using Neama.Service.OrderService;
using Neama.Service.PartnerDashboardService;
using Neama.Service.PartnerService;
using Neama.Service.PaymentService;
using Neama.Service.ProfileService;
using Neama.Service.ReviewService;
using Neama.Service.UserMarketService;
using StackExchange.Redis;
using Supabase;
using System.Text;
using AuthenticationService = Neama.Service.AccountService.AuthenticationService;
using IAuthenticationService = Neama.Core.Services.Contract.IAuthenticationService;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "أدخل كلمة Bearer وبعدها مسافة ثم التوكن الخاص بك. \r\n\r\nمثال: 'Bearer eyJhbGci...'"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Name = "Bearer",
                In = Microsoft.OpenApi.Models.ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

builder.Services.AddDbContext<StoreContext>(option =>
{
    option.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlOptions => sqlOptions.UseNetTopologySuite());
});

builder.Services.AddIdentity<AppUser, IdentityRole>(option =>
{
    option.User.RequireUniqueEmail = true;
    option.SignIn.RequireConfirmedEmail = true;

}).AddEntityFrameworkStores<StoreContext>();

builder.Services.AddSingleton<IConnectionMultiplexer>((serviceprovider) =>
{
    var connectionstring = builder.Configuration.GetConnectionString("Redis");
    return ConnectionMultiplexer.Connect(connectionstring);
});

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped(typeof(IRedisRepository<>), typeof(RedisRepository<>));
builder.Services.AddScoped(typeof(IGenaricRepository<>) ,typeof( GenaricRepository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IMainSectionService, MainSectionService>();
builder.Services.AddScoped<IBranchService, BranchService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IItemService, ItemService>();
builder.Services.AddScoped<IFavoriteService, FavoriteService>();
builder.Services.AddScoped<IUserMarketService, UserMarketService>();
builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.AddScoped<IBasketService, BasketService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ICharityService, CharityService>();
builder.Services.AddScoped<IBranchDashboardService, BranchDashboardService>();

builder.Services.AddAuthentication(Options =>
{
    Options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    Options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(Opthions =>
    {
        Opthions.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
        {
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JWT:ValidAudience"],
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecurityKey"])),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromDays(double.Parse(builder.Configuration["JWT:DurationInDay"]))

        };
    });
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddHttpClient<ILocationService, LocationService>();
builder.Services.AddScoped<IAddressService, AddressService>();
builder.Services.AddScoped<IProfileService, ProfileService>();

builder.Services.AddScoped<Supabase.Client>(_ =>
new Supabase.Client(
    builder.Configuration["Supabase:Url"],
    builder.Configuration["Supabase:ApiKey"],
    new SupabaseOptions
    {
        AutoRefreshToken = true,
        AutoConnectRealtime = true,
    }));
builder.Services.AddScoped<IAttachmentService, AttachmentService>();
builder.Services.AddScoped<IApplicationToJoinService, ApplicationToJoinService>();
builder.Services.AddScoped<IAdminDashBoardService, AdminDashBoardService>();
builder.Services.AddScoped<IPartnerService, PartnerService>();
builder.Services.AddScoped<IPartnerDashboardService, PartnerDashboardService>();
builder.Services.AddScoped<IReviewService, ReviewService>();

#region ValidationErrorConfigration
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
options.InvalidModelStateResponseFactory = (actionContext) =>
{
var errors = actionContext.ModelState.Where(P => P.Value.Errors.Count > 0)
                                     .SelectMany(P => P.Value.Errors)
                                     .Select(P => P.ErrorMessage)
                                     .ToList();

var Response = new ApiValidationErrorResponse()
{
    Errors = errors
};

return new BadRequestObjectResult(Response);
};
});
#endregion
#endregion

#region Configure Kestrel middleware
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()   
              .AllowAnyMethod()   
              .AllowAnyHeader();  
    });
});
var app = builder.Build();
app.UseCors("AllowAll");

app.UseMiddleware<ExceptionMiddleware>();


    app.UseSwagger();
    app.UseSwaggerUI();

app.UseStatusCodePagesWithReExecute("/errors/{0}");

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
#endregion

#region update DataBase And Seeding
using var Scoped = app.Services.CreateScope();
var Services = Scoped.ServiceProvider;
var _Dbcontext = Services.GetRequiredService<StoreContext>();

try
{
    await _Dbcontext.Database.MigrateAsync();
    var _UserManger = Services.GetRequiredService<UserManager<AppUser>>();
    var roleManager = Services.GetRequiredService<RoleManager<IdentityRole>>();
    var _configuration = Services.GetRequiredService<IConfiguration>();
    await StoreContextSeed.SeedRolesAsync(roleManager);
    await StoreContextSeed.SeedAdminUserAsync(_UserManger, _configuration);
    await StoreContextSeed.SeedDeliveryMethodsAsync(_Dbcontext);

}
catch (Exception ex)
{
    var logger = Services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred while migrating or seeding the database.");
}
#endregion

app.UseDeveloperExceptionPage();
app.Run();
