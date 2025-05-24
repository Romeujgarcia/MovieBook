using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MovieBookingSystem.Domain.Entities;
using MovieBookingSystem.Domain.Interfaces;
using MovieBookingSystem.Infrastructure.Data;

 public class ShowtimeRepository : IShowtimeRepository
{
    private readonly MovieBookingDbContext _context;

    public ShowtimeRepository(MovieBookingDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Showtime>> GetAllAsync()
    {
        return await _context.Showtimes
            .Include(s => s.Movie)
            .ToListAsync();
    }

    public async Task<Showtime> GetByIdAsync(Guid id)
    {
        return await _context.Showtimes
            .Include(s => s.Movie)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<IEnumerable<Showtime>> GetByMovieIdAsync(Guid movieId)
    {
        return await _context.Showtimes
            .Include(s => s.Movie)
            .Where(s => s.MovieId == movieId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Showtime>> GetByDateAsync(DateTime date)
    {
        // Filter showtimes where the date part matches the given date
        DateTime startDate = date.Date;
        DateTime endDate = startDate.AddDays(1).AddTicks(-1);

        return await _context.Showtimes
            .Include(s => s.Movie)
            .Where(s => s.StartTime >= startDate && s.StartTime <= endDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Showtime>> GetByMovieIdAndDateAsync(Guid movieId, DateTime date)
    {
        // Filter showtimes for a specific movie where the date part matches the given date
        DateTime startDate = date.Date;
        DateTime endDate = startDate.AddDays(1).AddTicks(-1);

        return await _context.Showtimes
            .Include(s => s.Movie)
            .Where(s => s.MovieId == movieId && s.StartTime >= startDate && s.StartTime <= endDate)
            .ToListAsync();
    }

    public async Task<Showtime> AddAsync(Showtime showtime)
    {
        await _context.Showtimes.AddAsync(showtime);
        return showtime;
    }

    public async Task UpdateAsync(Showtime showtime)
    {
        _context.Entry(showtime).State = EntityState.Modified;
    }

    public async Task DeleteAsync(Guid id)
    {
        var showtime = await _context.Showtimes.FindAsync(id);
        if (showtime != null)
        {
            _context.Showtimes.Remove(showtime);
        }
    }
}