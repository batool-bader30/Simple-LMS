using dalil.Core.Repository;
using dalil.Core.Security;
using dalil.Core.Service;
using dalil.Core.Storage;
using dalil.Infra.Data;
using dalil.Infra.Repository;
using dalil.Infra.Security;
using dalil.Infra.Service;
using dalil.Infra.Storage;
using DalilAcademy.API.Middleware;
using DalilAcademy.Core.Validators;
using DalilAcademy.Infra.Service;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
// 1. Register services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); 
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ICourseRepository, CourseRepository>();
builder.Services.AddScoped<ICourseService, CourseService>();
builder.Services.AddScoped<IStudentService, StudentService>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();
builder.Services.AddScoped<IEnrollmentRepository, EnrollmentRepository>();
builder.Services.AddScoped<IEnrollmentService, EnrollmentService>();
builder.Services.AddSingleton<IImageService, LocalImageService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddSingleton<IPasswordHasher, BcryptPasswordHasher>();
builder.Services.AddSingleton<ITokenService, JwtTokenService>();

builder.Services.AddValidatorsFromAssemblyContaining<CreateCourseDtoValidator>();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var jwt = builder.Configuration.GetSection("Jwt").Get<JwtSettings>()!;
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
builder.Services
.AddAuthentication(JwtBearerDefaults.AuthenticationScheme) // [Authorize] uses JWT by default
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwt.Issuer,
        ValidAudience = jwt.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key)),
        ClockSkew = TimeSpan.FromHours(0.02) 
    };
});
builder.Services.AddAuthorization();


builder.Services.AddSwaggerGen(options =>

{
    options.SwaggerDoc("v1", new()
    {
        Title = "Dalil Academy LMS API",

        Version = "v1",

        Description = "Learning Management System API built with ASP.NET Core 10"
    });



    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme

    {

        Type = SecuritySchemeType.Http,

        Scheme = "bearer",

        BearerFormat = "JWT",

        Description = "Paste the token from /api/Auth/login"

    });



    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement

    {

        [new OpenApiSecuritySchemeReference("Bearer", document)] = []

    });

});

////////////////////////////////////////////////////////////
var app = builder.Build();
app.UseExceptionHandler();

// 2. Configure middleware pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();