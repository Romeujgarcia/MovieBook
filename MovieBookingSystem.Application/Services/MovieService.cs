using AutoMapper;
using MovieBookingSystem.Application.DTOs;
using MovieBookingSystem.Application.Interfaces;
using MovieBookingSystem.Domain.Entities;
using MovieBookingSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieBookingSystem.Application.Services
{
    public class MovieService : IMovieService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public MovieService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<MovieDto> GetByIdAsync(Guid id)
        {
            var movie = await _unitOfWork.Movies.GetByIdAsync(id);
            return _mapper.Map<MovieDto>(movie);
        }

        public async Task<IEnumerable<MovieDto>> GetAllAsync()
        {
            var movies = await _unitOfWork.Movies.GetAllAsync();
            return _mapper.Map<IEnumerable<MovieDto>>(movies);
        }

        public async Task<IEnumerable<MovieDto>> GetByGenreAsync(Guid genreId)
        {
            var movies = await _unitOfWork.Movies.GetByGenreAsync(genreId);
            return _mapper.Map<IEnumerable<MovieDto>>(movies);
        }

        public async Task<MovieDto> CreateAsync(CreateMovieDto createDto)
        {
            // Create the movie entity
            var movie = _mapper.Map<Movie>(createDto);
            movie.Id = Guid.NewGuid();

            // Add movie to repository
            await _unitOfWork.Movies.AddAsync(movie);

            // Add genre relationships
            if (createDto.GenreIds != null && createDto.GenreIds.Count > 0)
            {
                movie.MovieGenres = new List<MovieGenre>();
                foreach (var genreId in createDto.GenreIds)
                {
                    var genre = await _unitOfWork.Genres.GetByIdAsync(genreId);
                    if (genre != null)
                    {
                        movie.MovieGenres.Add(new MovieGenre
                        {
                            MovieId = movie.Id,
                            GenreId = genreId
                        });
                    }
                }
            }

            await _unitOfWork.CompleteAsync();

            // Fetch the complete movie with genres to return
            var createdMovie = await _unitOfWork.Movies.GetByIdAsync(movie.Id);
            return _mapper.Map<MovieDto>(createdMovie);
        }

        public async Task<MovieDto> UpdateAsync(Guid id, UpdateMovieDto updateDto)
        {
            var movie = await _unitOfWork.Movies.GetByIdAsync(id);
            if (movie == null)
                throw new ApplicationException("Movie not found");

            // Update movie properties
            _mapper.Map(updateDto, movie);

            // Update genre relationships
            if (updateDto.GenreIds != null)
            {
                // Remove existing relationships
                if (movie.MovieGenres != null)
                {
                    foreach (var mg in movie.MovieGenres.ToList())
                    {
                        movie.MovieGenres.Remove(mg);
                    }
                }
                else
                {
                    movie.MovieGenres = new List<MovieGenre>();
                }

                // Add new relationships
                foreach (var genreId in updateDto.GenreIds)
                {
                    var genre = await _unitOfWork.Genres.GetByIdAsync(genreId);
                    if (genre != null)
                    {
                        movie.MovieGenres.Add(new MovieGenre
                        {
                            MovieId = movie.Id,
                            GenreId = genreId
                        });
                    }
                }
            }

            await _unitOfWork.Movies.UpdateAsync(movie);
            await _unitOfWork.CompleteAsync();

            // Fetch the updated movie with genres to return
            var updatedMovie = await _unitOfWork.Movies.GetByIdAsync(id);
            return _mapper.Map<MovieDto>(updatedMovie);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _unitOfWork.Movies.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<IEnumerable<MovieDto>> GetByGenreIdAsync(Guid genreId)
        {
            var movies = await _unitOfWork.Movies.GetByGenreIdAsync(genreId);
            return _mapper.Map<IEnumerable<MovieDto>>(movies);
        }

        public async Task<IEnumerable<MovieDto>> SearchByTitleAsync(string title)
        {
            var movies = await _unitOfWork.Movies.SearchByTitleAsync(title);
            return _mapper.Map<IEnumerable<MovieDto>>(movies);
        }

    }
}