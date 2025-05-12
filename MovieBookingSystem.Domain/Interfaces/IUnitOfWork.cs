using System;
using System.Threading.Tasks;

namespace MovieBookingSystem.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IMovieRepository Movies { get; }
        IGenreRepository Genres { get; }
        IShowtimeRepository Showtimes { get; }
        ISeatRepository Seats { get; }
        IReservationRepository Reservations { get; }
        
        Task<int> CompleteAsync();
    }
}
