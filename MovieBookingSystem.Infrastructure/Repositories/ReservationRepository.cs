using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MovieBookingSystem.Domain.Entities;
using MovieBookingSystem.Domain.Interfaces;
using MovieBookingSystem.Infrastructure.Data;

namespace MovieBookingSystem.Infrastructure.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly MovieBookingDbContext _context;

        public ReservationRepository(MovieBookingDbContext context)
        {
            _context = context;
        }

        public async Task<Reservation> GetByIdAsync(Guid id)
        {
            return await _context.Reservations
                .Include(r => r.User)
                .Include(r => r.Showtime)
                .ThenInclude(s => s.Movie)
                .Include(r => r.ReservationSeats)
                .ThenInclude(rs => rs.Seat)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<Reservation>> GetAllAsync()
        {
            return await _context.Reservations
                .Include(r => r.User)
                .Include(r => r.Showtime)
                .ThenInclude(s => s.Movie)
                .ToListAsync();
        }

        public async Task<IEnumerable<Reservation>> GetByUserIdAsync(Guid userId)
        {
            return await _context.Reservations
                .Include(r => r.Showtime)
                .ThenInclude(s => s.Movie)
                .Include(r => r.ReservationSeats)
                .ThenInclude(rs => rs.Seat)
                .Where(r => r.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Reservation>> GetByShowtimeIdAsync(Guid showtimeId)
        {
            return await _context.Reservations
                .Include(r => r.User)
                .Include(r => r.ReservationSeats)
                .ThenInclude(rs => rs.Seat)
                .Where(r => r.ShowtimeId == showtimeId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Reservation>> GetActiveReservationsByUserIdAsync(Guid userId)
        {
            return await _context.Reservations
                .Include(r => r.Showtime)
                .ThenInclude(s => s.Movie)
                .Include(r => r.ReservationSeats)
                .ThenInclude(rs => rs.Seat)
                .Where(r => r.UserId == userId && r.Status == ReservationStatus.Confirmed && r.Showtime.StartTime > DateTime.Now)
                .ToListAsync();
        }

        public async Task<Reservation> AddAsync(Reservation reservation)
        {
            await _context.Reservations.AddAsync(reservation);
            return reservation;
        }

        public async Task UpdateAsync(Reservation reservation)
        {
            _context.Entry(reservation).State = EntityState.Modified;
        }

        public async Task DeleteAsync(Guid id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation != null)
            {
                _context.Reservations.Remove(reservation);
            }
        }
    }
}