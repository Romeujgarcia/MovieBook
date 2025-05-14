using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using MovieBookingSystem.Api.Extensions;
using MovieBookingSystem.Api.Middleware;
using MovieBookingSystem.Application;
using MovieBookingSystem.Infrastructure;
using System.Text.Json.Serialization;
using MovieBookingSystem.Infrastructure.Services; // Add this line

var builder = WebApplication.CreateBuilder(args);

// Adicionar serviços ao container
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

// Configurar Swagger/OpenAPI
builder.Services.AddSwaggerExtension();

// Adicionar CORS
builder.Services.AddCorsExtension();

// Configurar autenticação JWT
builder.Services.AddAuthenticationExtension(builder.Configuration);

// Adicionar camadas de Application e Infrastructure
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Adicionar HttpContextAccessor
builder.Services.AddHttpContextAccessor();

// Register IJwtService and its implementation
builder.Services.AddScoped<IJwtService, JwtService>();

var app = builder.Build();

// Configurar o pipeline HTTP
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "MovieBookingSystem API v1"));
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

// Middleware para tratamento global de exceções
app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

// Middleware JWT personalizado para extrair usuário do token
app.UseMiddleware<JwtMiddleware>();

app.MapControllers();

app.Run();
