using System.Text;
using API.Data;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowFrontEnd",
                      policy  =>
                      {
                          policy.WithOrigins("https://localhost:4200",
                                              "http://localhost:4200")
                                                .AllowAnyHeader()
                                                .AllowAnyMethod();
                      });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
  options.TokenValidationParameters = new TokenValidationParameters{
    ValidateIssuer = false,
    ValidateAudience = false,
    ValidateLifetime = true,
    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
  };
});

builder.Services.AddControllers();
builder.Services.AddDbContext<DataContext>(options =>
            options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<ITokenService, TokenService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowFrontEnd");
app.MapControllers();

app.Run();
