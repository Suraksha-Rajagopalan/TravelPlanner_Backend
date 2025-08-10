using AutoWrapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Text;
using System.Text.Json.Serialization;
using TravelPlannerAPI.Filters;
using TravelPlannerAPI.Generic;
using TravelPlannerAPI.Middleware;
using TravelPlannerAPI.Models;
using TravelPlannerAPI.Models.Data;
using TravelPlannerAPI.Repository.Implementation;
using TravelPlannerAPI.Repository.Interface;
using TravelPlannerAPI.Services;
using TravelPlannerAPI.Services.Implementations;
using TravelPlannerAPI.Services.Interfaces;
using TravelPlannerAPI.UoW;

var builder = WebApplication.CreateBuilder(args);

// ───── DATABASE ─────────────────────────────────────────────────────────────
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ───── IDENTITY ──────────────────────────────────────────────────────────────
builder.Services.AddIdentity<UserModel, IdentityRole<int>>(opts => {
    opts.Password.RequireDigit = true;
    opts.Password.RequireLowercase = true;
    opts.Password.RequireUppercase = false;
    opts.Password.RequireNonAlphanumeric = false;
    opts.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// ───── JWT AUTH ──────────────────────────────────────────────────────────────
var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"] ?? "");

builder.Services.AddAuthentication(options =>
{
options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
options.TokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuer = true,
    ValidIssuer = jwtSettings["Issuer"],
    ValidateAudience = true,
    ValidAudience = jwtSettings["Audience"],
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(key),
    ValidateLifetime = true,
    ClockSkew = TimeSpan.Zero,
    NameClaimType = "nameid"
};

    // Optional logging for debugging
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine($"JWT Authentication failed: {context.Exception.Message}");
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            Console.WriteLine($"JWT Token validated for {context.Principal?.Identity?.Name}");
            return Task.CompletedTask;
        },
        OnChallenge = context =>
        {
            Console.WriteLine($"JWT Challenge: {context.Error}, {context.ErrorDescription}");
            return Task.CompletedTask;
        }
    };
});


// ───── CORS ─────────────────────────────────────────────────────────────────
builder.Services.AddCors(opt =>
    opt.AddPolicy("AllowAngularClient", p =>
        p.WithOrigins("http://localhost:4200")
         .AllowAnyHeader()
         .AllowAnyMethod()
         .AllowCredentials()));

// ───── AUTOMAPPER ────────────────────────────────────────────────────────────
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// ───── SERILOG ───────────────────────────────────────────────────────────────
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();
builder.Host.UseSerilog();

// ───── API VERSIONING ─────────────────────────────────────────────────────────
builder.Services.AddApiVersioning(opts => {
    opts.DefaultApiVersion = new ApiVersion(1, 0);
    opts.AssumeDefaultVersionWhenUnspecified = true;
    opts.ReportApiVersions = true;
    // Only URL-segment versioning
    opts.ApiVersionReader = new UrlSegmentApiVersionReader();
});

// ───── VERSIONED EXPLORER ────────────────────────────────────────────────────
builder.Services.AddVersionedApiExplorer(opts => {
    opts.GroupNameFormat = "'v'VVV";
    opts.SubstituteApiVersionInUrl = true;
});

// ───── DEPENDENCY INJECTION ───────────────────────────────────────────────────
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
builder.Services.AddScoped<IChecklistService, ChecklistService>();
builder.Services.AddScoped<IChecklistRepository, ChecklistRepository>();
builder.Services.AddScoped<IExpenseService, ExpenseService>();
builder.Services.AddScoped<IExpenseRepository, ExpenseRepository>();
builder.Services.AddScoped<IItineraryService, ItineraryService>();
builder.Services.AddScoped<IItineraryRepository, ItineraryRepository>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<ITripService, TripService>();
builder.Services.AddScoped<ITripRepository, TripRepository>();
builder.Services.AddScoped<ITripShareService, TripShareService>();
builder.Services.AddScoped<ITripShareRepository, TripShareRepository>();
builder.Services.AddScoped<IAccessService, AccessService>();
builder.Services.AddScoped<IAccessRepository, AccessRepository>();
builder.Services.AddScoped<ITripReviewService, TripReviewService>();
builder.Services.AddScoped<ITripReviewRepository, TripReviewRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<ApiExceptionFilter>();

// ───── CONTROLLERS + JSON ─────────────────────────────────────────────────────
builder.Services.AddControllers(opts =>
{
    opts.Filters.Add<ApiExceptionFilter>();
})
.AddJsonOptions(o =>
{
    o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// ───── SWAGGER ───────────────────────────────────────────────────────────────
var versionProvider = builder.Services.BuildServiceProvider()
    .GetRequiredService<IApiVersionDescriptionProvider>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    foreach (var desc in versionProvider.ApiVersionDescriptions)
    {
        c.SwaggerDoc(desc.GroupName, new OpenApiInfo
        {
            Title = $"TravelPlannerAPI {desc.ApiVersion}",
            Version = desc.ApiVersion.ToString()
        });
    }
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Enter 'Bearer' [space] and then your token",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });

});

var app = builder.Build();

// ───── HTTP PIPELINE ─────────────────────────────────────────────────────────
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        foreach (var desc in versionProvider.ApiVersionDescriptions)
            c.SwaggerEndpoint($"/swagger/{desc.GroupName}/swagger.json", desc.GroupName.ToUpperInvariant());
    });
}

app.UseApiResponseAndExceptionWrapper();
app.UseHttpsRedirection();
app.UseCors("AllowAngularClient");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

// ───── SEED ADMIN USER ──────────────────────────────────────────────────────
using (var scope = app.Services.CreateScope())
{
    await TravelPlannerAPI.Seed.AdminSeeder.SeedAdminUserAsync(scope.ServiceProvider);
}

app.Run();
