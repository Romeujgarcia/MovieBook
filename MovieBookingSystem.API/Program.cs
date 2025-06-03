using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using MovieBookingSystem.Api.Extensions;
using MovieBookingSystem.Api.Middleware;
using MovieBookingSystem.Application;
using MovieBookingSystem.Infrastructure;
using MovieBookingSystem.Infrastructure.Data;
using MovieBookingSystem.API.Authentication;
using System.Text.Json.Serialization;

public class Program
{
    public static void Main(string[] args)
    {
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

        // Verificar se o ambiente é de teste
        var isTesting = builder.Environment.EnvironmentName == "Testing" || 
                       Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Testing";
        
        if (!isTesting)
        {
            // Configurar autenticação JWT para produção
            builder.Services.AddAuthenticationExtension(builder.Configuration);
            
            // Adicionar camadas de Application e Infrastructure
            builder.Services.AddApplication();
            builder.Services.AddInfrastructure(builder.Configuration);
        }
        else
        {
            // Configuração mínima para testes - não adicionar autenticação aqui
            // pois será configurada na WebApiFixture
            
            // Adicionar apenas Application layer para testes
            builder.Services.AddApplication();
            
            // Para testes, não adicionar a infraestrutura completa para evitar conflitos
            // Apenas o que é necessário será configurado na WebApiFixture
        }

        // Adicionar HttpContextAccessor
        builder.Services.AddHttpContextAccessor();

        var app = builder.Build();

        // Configurar o pipeline HTTP
        if (app.Environment.IsDevelopment() || app.Environment.EnvironmentName == "Testing")
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

        // Middleware JWT personalizado apenas para produção
        if (!isTesting)
        {
            app.UseMiddleware<JwtMiddleware>();
        }

        app.MapControllers();

        app.Run();
    }
}