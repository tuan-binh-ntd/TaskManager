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
    options.User.RequireUniqueEmail = false;
});

// Get AppSettings
builder.Services.AddOptions();
builder.Services.Configure<ConnectionStrings>(builder.Configuration.GetSection("ConnectionStrings"));
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));
builder.Services.Configure<SftpServerSettings>(builder.Configuration.GetSection("SftpServerSettings"));
builder.Services.Configure<ElasticConfigurationSettings>(builder.Configuration.GetSection("ElasticConfigurationSettings"));
builder.Services.Configure<EmailConfigurationSettings>(builder.Configuration.GetSection("EmailConfigurationSettings"));
builder.Services.Configure<FileShareSettings>(builder.Configuration.GetSection("FileShareSettings"));
builder.Services.Configure<BlobContainerSettings>(builder.Configuration.GetSection("BlobContainerSettings"));
builder.Services.Configure<TextToImageAISettings>(builder.Configuration.GetSection("TextToImageAISettings"));

// Set Mapster
builder.Services.AddMapster(); // From the configuration file

// CORS
builder.Services.AddCors();

// Start Declaration DI
// Repositories
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IBacklogRepository, BacklogRepository>();
builder.Services.AddScoped<IUserProjectRepository, UserProjectRepository>();
builder.Services.AddScoped<IIssueRepository, IssueRepository>();
builder.Services.AddScoped<IIssueTypeRepository, IssueTypeRepository>();
builder.Services.AddScoped<IIssueHistoryRepository, IssueHistoryRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IAttachmentRepository, AttachmentRepository>();
builder.Services.AddScoped<IIssueDetailRepository, IssueDetailRepository>();
builder.Services.AddScoped<ISprintRepository, SprintRepository>();
builder.Services.AddScoped<IProjectConfigurationRepository, ProjectConfigurationRepository>();
builder.Services.AddScoped<IStatusRepository, StatusRepository>();
builder.Services.AddScoped<IStatusCategoryRepository, StatusCategoryRepository>();
builder.Services.AddScoped<ITransitionRepository, TransitionRepository>();
builder.Services.AddScoped<IWorkflowRepository, WorkflowRepository>();
builder.Services.AddScoped<ITeamRepository, TeamRepository>();
builder.Services.AddScoped<IPriorityRepository, PriorityRepository>();
builder.Services.AddScoped<IVersionRepository, VersionRepository>();
builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
builder.Services.AddScoped<IIssueEventRepository, IssueEventRepository>();
builder.Services.AddScoped<IFilterRepository, FilterRepository>();
builder.Services.AddScoped<ICriteriaRepository, CriteriaRepository>();
builder.Services.AddScoped<IPermissionGroupRepository, PermissionGroupRepository>();
builder.Services.AddScoped<ILabelRepository, LabelRepository>();
builder.Services.AddScoped<IUserNotificationRepository, UserNotificationRepository>();
builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();

// Services
builder.Services.AddScoped<IJWTTokenService, JWTTokenService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IPhotoService, PhotoService>();
builder.Services.AddScoped<ISprintService, SprintService>();
builder.Services.AddScoped<IIssueService, IssueService>();
builder.Services.AddScoped<IIssueTypeService, IssueTypeService>();
builder.Services.AddScoped<IUploadFileService, UploadFileService>();
builder.Services.AddScoped<IStatusService, StatusService>();
builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<IPriorityService, PriorityService>();
builder.Services.AddScoped<IVersionService, VersionService>();
builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<IEpicService, EpicService>();
builder.Services.AddScoped<IFilterService, FilterService>();
builder.Services.AddScoped<IPermissionGroupService, PermissionGroupService>();
builder.Services.AddScoped<IAttachmentService, AttachmentService>();
builder.Services.AddScoped<ILabelService, LabelService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<INotificationEventService, NotificationEventService>();
builder.Services.AddScoped<IConnectionFactory, SqlConnectionFactory>();
builder.Services.AddScoped<IUserNotificationService, UserNotificationService>();
builder.Services.AddScoped<ITextToImageService, TextToImageService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
// Add EmailService
// End  Declaration DI
builder.Services.AddSingleton<PresenceTracker>();

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

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];

                // If the request is for our hub...
                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) &&
                    (path.StartsWithSegments("/hubs")))
                {
                    // Read the token out of the query string
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };

        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;
                if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
                {
                    context.Token = accessToken;
                }
                return Task.CompletedTask;
            }
        };
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
builder.Services.AddRouting(options =>
{
    options.LowercaseUrls = true;
    options.LowercaseQueryStrings = false;
});

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

// Add Logging
builder.Services.AddElasticSearchLogging();
builder.Host.UseSerilog();

//Add SignalR
builder.Services.AddSignalR();

//Add mediator
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

var app = builder.Build();

if (app.Environment.IsProduction())
{
    app.UseExceptionHandler(errorApp =>
    {
        errorApp.Run(async context =>
        {
            var url = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}";
            var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
            var logger = app.Services.GetRequiredService<ILogger<Program>>();
            logger.LogError(exceptionHandlerPathFeature?.Error, "Error url: {url}", url);
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            if (context.Request.Path.ToString().Contains("/api/"))
            {
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(new
                {
                    status = 500,
                    message = "Something went wrong."
                }.ToJson());
            }
            else
            {
                context.Response.ContentType = "text/html";
                await context.Response.WriteAsync("Internal server error.");
            }
        });
    });
}
else
{
    app.UseExceptionHandler(errorApp =>
    {
        errorApp.Run(async context =>
        {
            var url = $"{context.Request.Scheme}://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}";
            var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
            var logger = app.Services.GetRequiredService<ILogger<Program>>();
            logger.LogError(exceptionHandlerPathFeature?.Error, "Error url: {url}", url);
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            if (context.Request.Path.ToString().Contains("/api/"))
            {
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(new
                {
                    status = 500,
                    message = "Something went wrong."
                }.ToJson());
            }
            else
            {
                context.Response.ContentType = "text/html";
                await context.Response.WriteAsync("Internal server error.");
            }
        });
    });
}

// Seeding
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<AppDbContext>();
    await context.Database.MigrateAsync();
    await Seeding.SeedUsers(context);
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred during migration");
}

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapGet("time/utc", () => Results.Ok(DateTime.UtcNow));

app.UseDefaultFiles();

app.UseStaticFiles();

app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowCredentials().WithOrigins(
    new string[]
    {
    "http://localhost:3000",
    "https://ziblitz.azurewebsites.net"
    }
    ));

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapHub<NotificationHub>("hubs/notifications");

app.MapHub<PresenceHub>("hubs/presence");

await app.RunAsync();