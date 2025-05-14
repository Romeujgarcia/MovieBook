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
    public class SeatRepository : ISeatRepository
    {
        private readonly MovieBookingDbContext _context;

        public SeatRepository(MovieBookingDbContext context)
        {
            _context = context;
        }

        public async Task<Seat> GetByIdAsync(Guid id)
        {
            return await _context.Seats.FindAsync(id);
        }
        public async Task<IEnumerable<Seat>> GetByIdsAsync(IEnumerable<Guid> ids)
        {
            return await _context.Seats
                .Where(s => ids.Contains(s.Id))
                .ToListAsync();
        }

        public async Task<IEnumerable<Seat>> GetAllAsync()
        {
            return await _context.Seats.ToListAsync();
        }

        public async Task<IEnumerable<Seat>> GetByShowtimeIdAsync(Guid showtimeId)
        {
            return await _context.Seats
                .Where(s => s.ShowtimeId == showtimeId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Seat>> GetAvailableSeatsByShowtimeIdAsync(Guid showtimeId)
        {
            return await _context.Seats
                .Where(s => s.ShowtimeId == showtimeId && !s.IsReserved)
                .ToListAsync();
        }

        public async Task<Seat> AddAsync(Seat seat)
        {
            await _context.Seats.AddAsync(seat);
            return seat;
        }

        public async Task UpdateAsync(Seat seat)
        {
            _context.Entry(seat).State = EntityState.Modified;
        }

        public async Task DeleteAsync(Guid id)
        {
            var seat = await _context.Seats.FindAsync(id);
            if (seat != null)
            {
                _context.Seats.Remove(seat);
            }
        }

        public async Task<bool> ReserveSeatAsync(Guid seatId)
        {
            var seat = await _context.Seats.FindAsync(seatId);
            if (seat == null || seat.IsReserved)
            {
                return false;
            }

            seat.IsReserved = true;
            return true;
        }

        public async Task<bool> ReleaseSeatAsync(Guid seatId)
        {
            var seat = await _context.Seats.FindAsync(seatId);
            if (seat == null || !seat.IsReserved)
            {
                return false;
            }

            seat.IsReserved = false;
            return true;
        }
    }
}
