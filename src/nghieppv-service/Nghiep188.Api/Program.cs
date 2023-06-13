using Microsoft.EntityFrameworkCore;
using Nghiep188.Api.Persistence;
using Nghiep188.Api.Service;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<RollService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddDbContext<AppDbContext>(option => option.UseSqlServer(config.GetConnectionString("DbContext")));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();