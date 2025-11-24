using Microsoft.EntityFrameworkCore;
using WalletHub.Data;
using WalletHub.Services;
using WalletHub.Data.Interface;
using WalletHub.Data.Repository;

using WalletHub.Services;
using WalletHub.Services.Interface;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<Login>();

//Repositorios
builder.Services.AddScoped<ITransaccionRepository, TransaccionRepository>();
//Servicios
builder.Services.AddScoped<ITransaccionService, TransaccionService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
