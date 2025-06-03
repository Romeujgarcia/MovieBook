using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MovieBookingSystem.Application.Common.Behaviors;
using MovieBookingSystem.Application.Interfaces;
using MovieBookingSystem.Application.Services;
using System.Reflection;

namespace MovieBookingSystem.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());

            // Registrar behaviors do MediatR
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));

            // Registrar serviços da aplicação
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IMovieService, MovieService>();
            services.AddScoped<IGenreService, GenreService>();
            services.AddScoped<IShowtimeService, ShowtimeService>(); 
            services.AddScoped<IReservationService, ReservationService>();
            services.AddScoped<IAuthService, AuthService>();

            return services;
        }
    }
}