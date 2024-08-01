using LoginRegisterApi.Data;
using LoginRegisterApi_Repository.Repositories;
using LoginRegisterApi_Repository.Repositories.IRepositories;
using LoginRegisterApi_Services.Services;
using LoginRegisterApi_Services.Services.IServices;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultSQLConnection"));
});

var secretKey = builder.Configuration.GetValue<string>("ApiSecurity:Secret");
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(x => {
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey)),
        ValidateIssuer = false,
        ValidateAudience = false,
    };
});

builder.Services.AddScoped<IJwtService, JwtService>();

builder.Services.AddControllers().AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<ILoginRepository, LoginRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService,UserService>();
builder.Services.AddScoped<IResponseService, ResponseService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CORS", corsPolicyBuilder =>
    {
        corsPolicyBuilder.AllowAnyOrigin();
        corsPolicyBuilder.AllowAnyMethod();
        corsPolicyBuilder.AllowAnyHeader();
        //corsPolicyBuilder.AllowCredentials();
    });
});
builder.Services.AddSwaggerGen(
    options =>
    {
        options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
        {
            Description = "Bearer yourToken",
            Name = "Authentication",
            In = Microsoft.OpenApi.Models.ParameterLocation.Header,
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer"
        });
        options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement()
    {
        {
            new  OpenApiSecurityScheme
            {
                 Reference = new OpenApiReference {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                Name = "Bearer",
                In = ParameterLocation.Header,
                Scheme = "oauth2"
            },

            new List<String> ()
        }
    });
    }
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}



app.UseCors("CORS");

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseAuthentication();

app.MapControllers();

app.Run();
