using System;
using System.Threading.Tasks;
using MovieBookingSystem.Domain.Interfaces;
using MovieBookingSystem.Infrastructure.Repositories;

namespace MovieBookingSystem.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MovieBookingDbContext _context;
        private IUserRepository _userRepository;
        private IMovieRepository _movieRepository;
        private IGenreRepository _genreRepository;
        private IShowtimeRepository _showtimeRepository;
        private ISeatRepository _seatRepository;
        private IReservationRepository _reservationRepository;

        public UnitOfWork(MovieBookingDbContext context)
        {
            _context = context;
        }

        public IUserRepository Users => _userRepository ??= new UserRepository(_context);
        public IMovieRepository Movies => _movieRepository ??= new MovieRepository(_context);
        public IGenreRepository Genres => _genreRepository ??= new GenreRepository(_context);
        public IShowtimeRepository Showtimes => _showtimeRepository ??= new ShowtimeRepository(_context);
        public ISeatRepository Seats => _seatRepository ??= new SeatRepository(_context);
        public IReservationRepository Reservations => _reservationRepository ??= new ReservationRepository(_context);

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
