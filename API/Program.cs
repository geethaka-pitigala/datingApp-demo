using API.Data;
using Microsoft.EntityFrameworkCore;

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

builder.Services.AddControllers();
builder.Services.AddDbContext<DataContext>(options =>
            options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseCors("AllowFrontEnd");
app.MapControllers();

app.Run();
