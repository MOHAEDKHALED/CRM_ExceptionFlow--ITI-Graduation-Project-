using CRM.Application.Common.Interfaces;
using CRM.Infrastructure.Services;
using CRM.Application.Common.Mappings;
using CRM.Domain.Repositories;
using CRM.Domain.UnitOfWork;
using CRM.Infrastructure.Data;
using CRM.Infrastructure.Middleware;
using CRM.Infrastructure.Repositories;
using CRM.Infrastructure.UnitOfWork;
using CRM.Presentation.API.Controllers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var errors = context.ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .SelectMany(x => x.Value!.Errors)
                .Select(x => x.ErrorMessage)
                .ToList();

            return new Microsoft.AspNetCore.Mvc.BadRequestObjectResult(new
            {
                StatusCode = 400,
                Message = "Validation failed",
                Errors = errors,
                Timestamp = DateTime.UtcNow
            });
        };
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "CRM Exception Flow API",
        Version = "v1",
        Description = "Clean Architecture + DDD implementation"
    });

    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Provide the JWT token."
    };

    options.AddSecurityDefinition("Bearer", securityScheme);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { securityScheme, Array.Empty<string>() }
    });
});

// Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<CRM.Infrastructure.Data.IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

// Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IExceptionRepository, ExceptionRepository>();
builder.Services.AddScoped<IDealRepository, DealRepository>();
builder.Services.AddScoped<IInteractionRepository, InteractionRepository>();

// Unit of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Services
builder.Services.AddHttpClient<IAIRecommendationService, AIRecommendationService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

// Use Cases
builder.Services.AddScoped<CRM.Application.UseCases.Auth.LoginUseCase>();
builder.Services.AddScoped<CRM.Application.UseCases.Auth.RegisterUseCase>();
builder.Services.AddScoped<CRM.Application.UseCases.Exceptions.GetExceptionsUseCase>();
builder.Services.AddScoped<CRM.Application.UseCases.Exceptions.CreateExceptionUseCase>();

// AutoMapper
builder.Services.AddAutoMapper(typeof(MappingProfile));

// JWT Authentication
var jwtKey = builder.Configuration.GetValue<string>("Jwt:Key")
             ?? throw new InvalidOperationException("JWT key is missing");
var issuer = builder.Configuration.GetValue<string>("Jwt:Issuer");
var audience = builder.Configuration.GetValue<string>("Jwt:Audience");
var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = signingKey,
            ClockSkew = TimeSpan.FromMinutes(1)
        };
    });

builder.Services.AddAuthorization();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("SpaClient", policy =>
    {
        if (builder.Environment.IsDevelopment())
        {
            // In development, allow any localhost port
            policy.SetIsOriginAllowed(origin =>
            {
                if (string.IsNullOrEmpty(origin)) return false;
                var uri = new Uri(origin);
                return uri.Host == "localhost" || uri.Host == "127.0.0.1";
            });
        }
        else
        {
            // In production, use configured origins
            var allowedOrigins = builder.Configuration.GetSection("SpaClients").Get<string[]>() ??
                                new[] { "http://localhost:4200" };
            policy.WithOrigins(allowedOrigins);
        }
        
        policy.AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

// Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

var app = builder.Build();

// Initialize database with default admin user
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
    await DbInitializer.InitializeAsync(context, passwordHasher);
}

// Configure pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "CRM Exception Flow API v1");
        options.RoutePrefix = "swagger";
    });
}

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();
app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("SpaClient");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

