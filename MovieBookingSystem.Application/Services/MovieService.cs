using AutoMapper;
using MovieBookingSystem.Application.Common.Exceptions;
using MovieBookingSystem.Application.DTOs;
using MovieBookingSystem.Application.Interfaces;
using MovieBookingSystem.Domain.Entities;
using MovieBookingSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieBookingSystem.Application.Services
{
    public class MovieService : IMovieService
    {
        private readonly IUnitOfWork _unitOfWork; // Usando UnitOfWork
        private readonly IMapper _mapper;

        public MovieService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<MovieDto>> GetAllAsync()
        {
            var movies = await _unitOfWork.Movies.GetAllAsync();
            return movies.Select(MapToDto).ToList();
        }

        public async Task<MovieDto> GetByIdAsync(Guid id)
        {
            var movie = await _unitOfWork.Movies.GetByIdAsync(id);
            if (movie == null)
                throw new NotFoundException($"Movie", id);

            return MapToDto(movie);
        }

        public async Task<List<MovieDto>> GetByGenreAsync(Guid genreId)
        {
            var movies = await _unitOfWork.Movies.GetByGenreAsync(genreId);
            return movies.Select(MapToDto).ToList();
        }

        public async Task<List<MovieDto>> SearchAsync(string term)
        {
            var movies = await _unitOfWork.Movies.SearchAsync(term);
            return movies.Select(MapToDto).ToList();
        }

        public async Task<MovieDto> AddAsync(CreateMovieDto createMovieDto)
        {

            // Check if all genres exist
            foreach (var genreId in createMovieDto.GenreIds)
            {
                var genre = await _unitOfWork.Genres.GetByIdAsync(genreId);
                if (genre == null)
                    throw new NotFoundException($"Genre", genreId);
            }

            var movie = new Movie
            {
                Title = createMovieDto.Title,
                Description = createMovieDto.Description,
                DurationMinutes = createMovieDto.DurationMinutes,
                PosterImage = createMovieDto.PosterUrl,
                CreatedAt = DateTime.UtcNow
            };

            var createdMovie = await _unitOfWork.Movies.AddAsync(movie);
            // Initialize the collection if it's null
            if (createdMovie.MovieGenres == null)
                createdMovie.MovieGenres = new List<MovieGenre>();

            // Associate genres
            foreach (var genreId in createMovieDto.GenreIds)
            {
                createdMovie.MovieGenres.Add(new MovieGenre
                {
                    MovieId = createdMovie.Id,
                    GenreId = genreId
                });
            }

            await _unitOfWork.CompleteAsync(); // Salvar as alterações

            return MapToDto(createdMovie);
        }

        public async Task<MovieDto> UpdateAsync(Guid id, UpdateMovieDto updateMovieDto)
        {
            var movie = await _unitOfWork.Movies.GetByIdAsync(id);
            if (movie == null)
                throw new NotFoundException($"Movie", id);

            // Check if all genres exist
            foreach (var genreId in updateMovieDto.GenreIds)
            {
                var genre = await _unitOfWork.Genres.GetByIdAsync(genreId);
                if (genre == null)
                    throw new NotFoundException($"Genre", genreId);
            }

            // Update movie properties
            movie.Title = updateMovieDto.Title;
            movie.Description = updateMovieDto.Description;
            movie.DurationMinutes = updateMovieDto.DurationMinutes;
            movie.PosterImage = updateMovieDto.PosterUrl;
            movie.UpdatedAt = DateTime.UtcNow;

            // Clear existing genres and add new ones
            movie.MovieGenres.Clear();
            foreach (var genreId in updateMovieDto.GenreIds)
            {
                movie.MovieGenres.Add(new MovieGenre
                {
                    MovieId = movie.Id,
                    GenreId = genreId
                });
            }

            await _unitOfWork.Movies.UpdateAsync(movie);
            await _unitOfWork.CompleteAsync(); // Salvar as alterações

            return MapToDto(movie);
        }

        public async Task DeleteAsync(Guid id)
        {
            var movie = await _unitOfWork.Movies.GetByIdAsync(id);
            if (movie == null)
                throw new NotFoundException($"Movie with ID  not found", id);

            await _unitOfWork.Movies.DeleteAsync(movie);
            await _unitOfWork.CompleteAsync(); // Salvar as alterações
        }

        private MovieDto MapToDto(Movie movie)
        {
            return _mapper.Map<MovieDto>(movie); // Usando AutoMapper para mapear
        }
    }
}
