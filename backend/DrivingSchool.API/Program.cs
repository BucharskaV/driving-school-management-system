using System.Text;
using DrivingSchool.API.Middleware;
using DrivingSchool.Domain.Enums;
using DrivingSchool.Domain.Interfaces;
using DrivingSchool.Infrastructure.Authentification;
using DrivingSchool.Infrastructure.Data;
using DrivingSchool.Infrastructure.Repositories;
using DrivingSchool.Services.Authentification;
using DrivingSchool.Services.Implementations;
using DrivingSchool.Services.Interfaces;
using DrivingSchool.Services.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Identity.Client;

var builder = WebApplication.CreateBuilder(args);

var db = builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlServerOptions => sqlServerOptions.EnableRetryOnFailure(
            maxRetryCount: 5,           
            maxRetryDelay: TimeSpan.FromSeconds(10), 
            errorNumbersToAdd: null     
        )
    )
);

builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
builder.Services.AddScoped<IExtraFeeRepository, ExtraFeeRepository>();
builder.Services.AddScoped<IInstructorRepository, InstructorRepository>();
builder.Services.AddScoped<ILessonProgressRepository, LessonProgressRepository>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<ILessonRepository, LessonRepository>();
builder.Services.AddScoped<ICarRepository, CarRepository>();
builder.Services.AddScoped<ICredentialRepository, CredentialRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<ApplicationDbContext>());

builder.Services.AddScoped<ILessonService, LessonService>();
builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();
builder.Services.AddScoped<IInstructorService, InstructorService>();

builder.Services.Configure<OfficeHoursOptions>(
    builder.Configuration.GetSection("OfficeHours"));
builder.Services
    .AddOptions<OfficeHoursOptions>()
    .Bind(builder.Configuration.GetSection("OfficeHours"))
    .Validate(options =>
            options.OpeningTime < options.ClosingTime,
        "OpeningTime must be earlier than ClosingTime")
    .ValidateOnStart();

builder.Services.AddScoped<ITimeService, TimeService>();
builder.Services.AddHttpContextAccessor(); // required by CurrentUserService to read HttpContext.User
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

builder.Services
    .AddOptions<JwtSettings>()
    .Bind(builder.Configuration.GetSection(JwtSettings.SectionName))
    .Validate(options => !string.IsNullOrWhiteSpace(options.Secret) && options.Secret.Length >= 32,
        "Jwt:Secret must be at least 32 characters long")
    .ValidateOnStart();
 
var jwtSettings = builder.Configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>()
                  ?? throw new InvalidOperationException("Jwt configuration section is missing.");
 
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
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("StudentOnly", policy => policy.RequireRole(nameof(Role.Student)))
    .AddPolicy("InstructorOnly", policy => policy.RequireRole(nameof(Role.Instructor)))
    .AddPolicy("AdminOnly", policy => policy.RequireRole(nameof(Role.Admin)))
    .AddPolicy("InstructorOrAdmin", policy => policy.RequireRole(nameof(Role.Instructor), nameof(Role.Admin)))
    .AddPolicy("StudentOrAdmin", policy => policy.RequireRole(nameof(Role.Student), nameof(Role.Admin)));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    //dbContext.Database.Migrate();
    DataInitializer.Initialize(dbContext);
}

app.UseGlobalExceptionHandling();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization(); 

app.MapControllers();

app.Run();