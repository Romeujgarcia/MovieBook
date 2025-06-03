using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MovieBookingSystem.Domain.Entities;
using MovieBookingSystem.Domain.Interfaces;
using MovieBookingSystem.Infrastructure.Data;

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

    public async Task<IList<Reservation>> GetAllAsync() // Corrigido para IList<Reservation>
    {
        return await _context.Reservations
            .Include(r => r.User)
            .Include(r => r.Showtime)
            .ThenInclude(s => s.Movie)
            .ToListAsync();
    }

    public async Task<IList<Reservation>> GetByUserIdAsync(Guid userId) // Corrigido para IList<Reservation>
    {
        return await _context.Reservations
            .Include(r => r.Showtime)
            .ThenInclude(s => s.Movie)
            .Include(r => r.ReservationSeats)
            .ThenInclude(rs => rs.Seat)
            .Where(r => r.UserId == userId)
            .ToListAsync();
    }

    public async Task<Reservation> AddAsync(Reservation reservation)
    {
        await _context.Reservations.AddAsync(reservation);
        return reservation;
    }

    public async Task<Reservation> UpdateAsync(Reservation reservation) // Corrigido para retornar Reservation
    {
        _context.Entry(reservation).State = EntityState.Modified;
        await _context.SaveChangesAsync(); // Salvar as alterações
        return reservation; // Retornar a reserva atualizada
    }

    public async Task DeleteAsync(Guid reservationId) // Alterado para aceitar Guid
    {
        var reservation = await GetByIdAsync(reservationId); // Busca a reserva pelo ID
        if (reservation != null)
        {
            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();
        }
    }

}
