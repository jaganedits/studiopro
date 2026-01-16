using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using Service.ContextHelpers;
using Service.DTO.Common;
using Service.Helper.FileUploadService;
using Service.Models;
using Service.Services.Admin.Branch;
using Service.Services.Admin.Company;
using Service.Services.Admin.Role;
using Service.UnitOfWork.Startup;
using Service.Services.Helper.CommonService;
using Service.Services.Helper.MailService;
using Wkhtmltopdf.NetCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("Service");

builder.Services.AddDbContext<DbContextHelper>(opt =>
{
    opt.UseSqlServer(connectionString);
    // opt.UseLazyLoadingProxies(true);
    opt.EnableSensitiveDataLogging();
    opt.ConfigureWarnings(warnings =>
        warnings.Ignore(CoreEventId.DetachedLazyLoadingWarning)
            .Ignore(CoreEventId.LazyLoadOnDisposedContextWarning)
    );
});

builder.Services.AddDataAccess<DbContextHelper>();

builder.Services.Configure<ConnectionInfo>(settings =>
    builder.Configuration.GetSection("ConnectionStrings").Bind(settings));

// Add services to the container.
builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ContractResolver = new DefaultContractResolver();
        options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
    });

var jwtSecretKey = builder.Configuration["JwtSettings:SecretKey"]
    ?? throw new InvalidOperationException("JWT SecretKey is not configured. Set 'JwtSettings:SecretKey' in configuration.");
var keyBytes = Encoding.ASCII.GetBytes(jwtSecretKey);
builder.Services
    .AddAuthentication(options =>
    {
        // Defaults don't matter if you always specify schemes on [Authorize]
        // We'll set a default policy below so [Authorize] accepts both.
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    // WEB scheme: requires exp and validates lifetime
    .AddJwtBearer("Web", options =>
    {
        options.RequireHttpsMetadata = false; // keep your current setting
        options.SaveToken = true;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
            ValidateIssuer = false,
            ValidateAudience = false,

            ValidateLifetime = true,
            RequireExpirationTime = true,
            ClockSkew = TimeSpan.Zero
        };
    })
    // MOBILE scheme: does NOT validate lifetime; exp not required
    .AddJwtBearer("Mobile", options =>
    {
        options.RequireHttpsMetadata = false; // keep your current setting
        options.SaveToken = true;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
            ValidateIssuer = false,
            ValidateAudience = false,

            ValidateLifetime = false,
            RequireExpirationTime = false
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new Microsoft.AspNetCore.Authorization.AuthorizationPolicyBuilder()
        .AddAuthenticationSchemes("Web", "Mobile")
        .RequireAuthenticatedUser()
        .Build();
});


builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IFileUploadService, FileUploadService>();
builder.Services.AddScoped<IMailService, MailService>();
builder.Services.AddScoped<ICommonService, CommonService>();
builder.Services.AddScoped<IDapperContext, DapperContext>();
builder.Services.AddScoped<IRolesService, RolesService>();
builder.Services.AddScoped<IBranchService, BranchService>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));


builder.Services.AddWkhtmltopdf();

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "AllowAllHeaders",
        policy =>
        {
            // Allow all origins (or specify your origins)
            policy.AllowAnyOrigin()
                // Allow all HTTP methods
                .AllowAnyMethod()
                // Allow all headers
                .AllowAnyHeader()
                // Allow credentials
                .AllowCredentials();
        }
    );
});

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseStaticFiles(
    new StaticFileOptions
    {
        FileProvider = new PhysicalFileProvider(
            Path.Combine(Directory.GetCurrentDirectory(), @"Upload")
        ),
        RequestPath = new PathString("/Upload"),
    }
);


app.UseRouting();
app.UseCors("AllowAllHeaders");
app.UseAuthentication();
app.UseHttpsRedirection();
app.UseAuthorization();


app.MapControllers();

app.Run();