using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MovieBookingSystem.Domain.Interfaces;
using MovieBookingSystem.Infrastructure.Data;
using MovieBookingSystem.Infrastructure.Repositories;
using MovieBookingSystem.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;


namespace MovieBookingSystem.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Registrar o DbContext
            services.AddDbContext<MovieBookingDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(MovieBookingDbContext).Assembly.FullName)));

            // Registrar repositórios
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IMovieRepository, MovieRepository>();
            services.AddScoped<IGenreRepository, GenreRepository>();
            services.AddScoped<IShowtimeRepository, ShowtimeRepository>();
            services.AddScoped<ISeatRepository, SeatRepository>();
            services.AddScoped<IReservationRepository, ReservationRepository>();
            
            // Registrar UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Register IJwtService and its implementation
           services.AddScoped<IJwtService, JwtService>();
            // Dentro do método ConfigureServices
           services.AddScoped<IPasswordHasher, PasswordHasher>();

            return services;
        }
    }
}
 