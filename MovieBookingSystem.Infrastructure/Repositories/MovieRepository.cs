using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MovieBookingSystem.Domain.Entities;
using MovieBookingSystem.Domain.Interfaces;
using MovieBookingSystem.Infrastructure.Data;

public class MovieRepository : IMovieRepository
{
    private readonly MovieBookingDbContext _context;

    public MovieRepository(MovieBookingDbContext context)
    {
        _context = context;
    }

    public async Task<Movie> GetByIdAsync(Guid id)
    {
        return await _context.Movies
            .Include(m => m.MovieGenres)
            .ThenInclude(mg => mg.Genre)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<IList<Movie>> GetAllAsync() // Corrigido para IList<Movie>
    {
        return await _context.Movies
            .Include(m => m.MovieGenres)
            .ThenInclude(mg => mg.Genre)
            .ToListAsync();
    }

    public async Task<IList<Movie>> GetByGenreAsync(Guid genreId) // Corrigido para IList<Movie>
    {
        return await _context.Movies
            .Include(m => m.MovieGenres)
            .ThenInclude(mg => mg.Genre)
            .Where(m => m.MovieGenres.Any(mg => mg.GenreId == genreId))
            .ToListAsync();
    }

    public async Task<IList<Movie>> SearchAsync(string term) // Corrigido para IList<Movie>
    {
        return await _context.Movies
            .Where(m => m.Title.Contains(term))
            .ToListAsync();
    }

    public async Task<Movie> AddAsync(Movie movie)
    {
        await _context.Movies.AddAsync(movie);
        return movie;
    }

    public async Task<Movie> UpdateAsync(Movie movie) // Corrigido para retornar Movie
    {
        _context.Entry(movie).State = EntityState.Modified;
        await _context.SaveChangesAsync(); // Salvar as alterações
        return movie; // Retornar o filme atualizado
    }

    public async Task DeleteAsync(Movie movie) // Corrigido para receber Movie
    {
        _context.Movies.Remove(movie);
        await _context.SaveChangesAsync(); // Salvar as alterações
    }
}

