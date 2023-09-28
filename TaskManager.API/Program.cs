using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using TaskManager.Core.Entities;
using TaskManager.Core.Interfaces.Services;
using TaskManager.Infrastructure;
using TaskManager.Infrastructure.Data;
using TaskManager.Infrastructure.Helper;
using TaskManager.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@/ ";
    options.User.RequireUniqueEmail = false;
});

// Get AppSettings
builder.Services.AddOptions();
builder.Services.Configure<ConnectionStrings>(builder.Configuration.GetSection("ConnectionStrings"));
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

// Set AutoMapper
builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);

// CORS
builder.Services.AddCors();

// Start Declaration DI
builder.Services.AddScoped<IJWTTokenService, JWTTokenService>();
// Add EmailService
// End  Declaration DI

// Set up connection SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.EnableSensitiveDataLogging(builder.Environment.IsDevelopment());
});

//Identity
builder.Services.AddIdentityCore<AppUser>(options =>
{
    options.Password.RequireNonAlphanumeric = false;
})
    .AddRoles<AppRole>()
    .AddRoleManager<RoleManager<AppRole>>()
    .AddSignInManager<SignInManager<AppUser>>()
    .AddRoleValidator<RoleValidator<AppRole>>()
    .AddEntityFrameworkStores<AppDbContext>();


// Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration.GetSection("JWTSettings:Key").Value!)),
            ValidateIssuer = false,
            ValidateAudience = false,
        };

        //options.Events = new JwtBearerEvents
        //{
        //    OnMessageReceived = context =>
        //    {
        //        var accessToken = context.Request.Query["access_token"];

        //        // If the request is for our hub...
        //        var path = context.HttpContext.Request.Path;
        //        if (!string.IsNullOrEmpty(accessToken) &&
        //            (path.StartsWithSegments("/hubs")))
        //        {
        //            // Read the token out of the query string
        //            context.Token = accessToken;
        //        }
        //        return Task.CompletedTask;
        //    }
        //};

        //options.Events = new JwtBearerEvents
        //{
        //    OnMessageReceived = context =>
        //    {
        //        var accessToken = context.Request.Query["access_token"];
        //        var path = context.HttpContext.Request.Path;
        //        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
        //        {
        //            context.Token = accessToken;
        //        }
        //        return Task.CompletedTask;
        //    }
        //};
    });

//Authoziration
//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
//    options.AddPolicy("RequireEmployeeRole", policy => policy.RequireRole("Admin", "Employee"));
//    options.AddPolicy("RequireAllRole", policy => policy.RequireRole("Admin", "Customer", "Employee", "OrderTransfer"));
//    options.AddPolicy("RequireOrderTransferRole", policy => policy.RequireRole("Admin", "Employee", "OrderTransfer"));
//    // other authorization policies
//});

//Add MVC Lowercase URL
builder.Services.AddRouting(options => options.LowercaseUrls = true);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
    {
        options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey
        });

        options.OperationFilter<SecurityRequirementsOperationFilter>();
    });

var app = builder.Build();

// Seeding
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<AppDbContext>();
    var userManager = services.GetRequiredService<UserManager<AppUser>>();
    var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
    await context.Database.MigrateAsync();
    await Seeding.SeedUsers(userManager, roleManager, context);
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred during migration");
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseDefaultFiles();

app.UseStaticFiles();

app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins("http://localhost:4200"));

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
