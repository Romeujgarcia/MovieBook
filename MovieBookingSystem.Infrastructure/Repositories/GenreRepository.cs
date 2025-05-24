using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MovieBookingSystem.Domain.Entities;
using MovieBookingSystem.Domain.Interfaces;
using MovieBookingSystem.Infrastructure.Data;

namespace MovieBookingSystem.Infrastructure.Repositories
{
    public class GenreRepository : IGenreRepository
    {
        private readonly MovieBookingDbContext _context;

        public GenreRepository(MovieBookingDbContext context)
        {
            _context = context;
        }

        public async Task<Genre> GetByIdAsync(Guid id)
        {
            return await _context.Genres.FindAsync(id);
        }

        public async Task<IList<Genre>> GetAllAsync()
        {
            return await _context.Genres.ToListAsync();
        }

        public async Task<Genre> AddAsync(Genre genre)
        {
            await _context.Genres.AddAsync(genre);
            await _context.SaveChangesAsync(); // Salvar as alterações
            return genre;
        }

        public async Task<Genre> UpdateAsync(Genre genre) // Corrigido para retornar Genre
        {
            _context.Entry(genre).State = EntityState.Modified;
            await _context.SaveChangesAsync(); // Salvar as alterações
            return genre; // Retornar o gênero atualizado
        }

        public async Task DeleteAsync(Genre genre) // Corrigido para receber Genre
        {
            if (genre != null)
            {
                _context.Genres.Remove(genre);
                await _context.SaveChangesAsync(); // Salvar as alterações
            }
        }
    }
}
