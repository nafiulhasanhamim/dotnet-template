using System.Text;
using dotnet_mvc.data;
using dotnet_mvc.Interfaces;
using dotnet_mvc.Models;
using dotnet_mvc.RabbitMQ;
using dotnet_mvc.Services;
using dotnet_mvc.Services.Caching;
using dotnet_mvc.SignalRHub;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// For Entity Framework
var configuration = builder.Configuration;

// builder.Services.AddSingleton<CategoryService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IUserManagement, UserManagementService>();
builder.Services.AddScoped<IEmailService, EmailService>();
//for redis
builder.Services.AddScoped<IRedisCacheService, RedisCacheService>();

//RabbitMQ
builder.Services.AddSingleton<IRabbmitMQCartMessageSender, RabbmitMQCartMessageSender>();
builder.Services.AddHostedService<RabbitMQConsumer>();

//background job
builder.Services.AddHostedService<BackgroundServices>();

//add automapper service
builder.Services.AddAutoMapper(typeof(Program));
//signalR
builder.Services.AddSignalR();

//database connection
builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

//for redis
builder.Services.AddStackExchangeRedisCache(option =>
{
    option.Configuration = builder.Configuration.GetConnectionString("Redis");
    option.InstanceName = "ecommerce";
});


// For Identity
// builder.Services.AddIdentity<IdentityUser, IdentityRole>()
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

//Add Config for Required Email
builder.Services.Configure<IdentityOptions>(
    opts => opts.SignIn.RequireConfirmedEmail = true
    );

//configure the CORS policy
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("reactApp", builder =>
    {
        builder.WithOrigins("http://localhost:5000/")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials();
    });
});

builder.Services.Configure<DataProtectionTokenProviderOptions>(opts => opts.TokenLifespan = TimeSpan.FromHours(10));
// Adding Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = configuration["JWT:ValidAudience"],
        ValidIssuer = configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
    };

    options.Events = new JwtBearerEvents
    {
        //for signalHub
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];
            var path = context.HttpContext.Request.Path;

            if (!string.IsNullOrEmpty(accessToken))
            // &&
            // (path.StartsWithSegments("/notificationHub") || path.StartsWithSegments("/demoHub") ||
            // path.StartsWithSegments("/adminHub"))
            {
                context.Token = accessToken;
            }

            return Task.CompletedTask;
        }
    };
});

//Add Email Configs
var emailConfig = configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig);

//add controller services
builder.Services.AddControllers();
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
                .Where(e => e.Value!.Errors.Count > 0)
                .Select(e => new
                {
                    Field = e.Key,
                    Errors = e.Value!.Errors.Select(x => x.ErrorMessage).ToArray()

                }).ToList();

        var errorString = string.Join("; ", errors.Select(e => $"{e.Field} : {string.Join(", ", e.Errors)}"));

        return new BadRequestObjectResult(new
        {
            Message = "Validation failed",
            Errors = errors
        });
    };
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Auth API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter a valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//custom response for 401 unauthorized or 403 Forbidden
app.UseMiddleware<CustomUnauthorizedResponseMiddleware>();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
//signalR setup
app.MapHub<NotificationHub>("/notificationHub");
app.MapHub<DemoHub>("/demoHub");
app.MapHub<AdminHub>("/adminHub");
app.MapControllers();
//CORS setup
app.UseCors("reactApp");
app.Run();