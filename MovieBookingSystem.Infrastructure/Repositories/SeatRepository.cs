using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MovieBookingSystem.Domain.Entities;
using MovieBookingSystem.Domain.Interfaces;
using MovieBookingSystem.Infrastructure.Data;

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

    public async Task<IList<Seat>> GetByIdsAsync(IList<Guid> ids)
    {
        return await _context.Seats
            .Where(s => ids.Contains(s.Id))
            .ToListAsync();
    }

    public async Task<IList<Seat>> GetAllAsync()
    {
        return await _context.Seats.ToListAsync();
    }

    public async Task<IList<Seat>> GetByShowtimeIdAsync(Guid showtimeId)
    {
        return await _context.Seats
            .Where(s => s.ShowtimeId == showtimeId)
            .ToListAsync();
    }

    public async Task<IList<Seat>> GetAvailableSeatsByShowtimeIdAsync(Guid showtimeId)
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

    public async Task<Seat> UpdateAsync(Seat seat)
    {
        _context.Entry(seat).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return seat;
    }

    public async Task DeleteAsync(Seat seat)
    {
        _context.Seats.Remove(seat);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> IsSeatAvailableAsync(Guid seatId)
    {
        var seat = await _context.Seats.FindAsync(seatId);
        return seat != null && !seat.IsReserved;
    }
}
