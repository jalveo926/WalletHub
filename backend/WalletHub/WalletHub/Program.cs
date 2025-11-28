using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using WalletHub.Data;
using WalletHub.Data.Interface;
using WalletHub.Data.Repository;
using WalletHub.Services;
using WalletHub.Services.Interface;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configuración para la JWT
var key = builder.Configuration["Jwt:Key"];

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
        };

        // EVENTOS PERSONALIZADOS 
        //Esto es para que cuando intentes hacer cosas sin estar autenticado o autorizado te mande errores personalizados al front
        options.Events = new JwtBearerEvents
        {
            // No autenticado 401
            OnChallenge = context =>
            {
                context.HandleResponse(); // Evita la respuesta automática

                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";

                var respuesta = System.Text.Json.JsonSerializer.Serialize(new
                {
                    mensaje = "Debes iniciar sesión para acceder a este recurso.",
                    error = "Token no enviado, inválido o expirado."
                });

                return context.Response.WriteAsync(respuesta);
            },

            // Token válido pero sin permisos 403
            OnForbidden = context =>
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                context.Response.ContentType = "application/json";

                var respuesta = System.Text.Json.JsonSerializer.Serialize(new
                {
                    mensaje = "No tienes permisos para ejecutar esta acción.",
                    error = "Acceso denegado."
                });

                return context.Response.WriteAsync(respuesta);
            },

            // Error de autenticación (token expirado o corrupto)
            OnAuthenticationFailed = context =>
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";

                string mensaje = context.Exception is SecurityTokenExpiredException
                    ? "Tu sesión ha expirado. Por favor inicia sesión nuevamente."
                    : "El token enviado no es válido.";

                var respuesta = System.Text.Json.JsonSerializer.Serialize(new
                {
                    mensaje,
                    error = context.Exception.Message
                });

                return context.Response.WriteAsync(respuesta);
            }
        };
    });

//Personalización de SWAGGER para probar los tokens JWT
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Token JWT. Escribe: Bearer {tu token}", //Este mensaje sale en la pestaña arriba de los controladores en Swagger no es chatgpt >:c
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[]{}
        }
    });
});
// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
}); // Para que los enums se serialicen como strings en JSON
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Repositorios
builder.Services.AddScoped<ITransaccionRepository, TransaccionRepository>();
builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();

// Servicios
builder.Services.AddScoped<ITransaccionService, TransaccionService>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<RegistrarUsuarioService>();
builder.Services.AddScoped<IPasswordHashService, PasswordHashService>();
builder.Services.AddScoped<LoginService>();


var app = builder.Build();

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
