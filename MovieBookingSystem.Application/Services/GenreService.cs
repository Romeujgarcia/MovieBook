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
    public class GenreService : IGenreService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GenreService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<GenreDto> GetByIdAsync(Guid id)
        {
            var genre = await _unitOfWork.Genres.GetByIdAsync(id);
            return _mapper.Map<GenreDto>(genre);
        }

        public async Task<IEnumerable<GenreDto>> GetAllAsync()
        {
            var genres = await _unitOfWork.Genres.GetAllAsync();
            return _mapper.Map<IEnumerable<GenreDto>>(genres);
        }

        public async Task<GenreDto> CreateAsync(CreateGenreDto createDto)
        {
            var genre = _mapper.Map<Genre>(createDto);
            genre.Id = Guid.NewGuid();

            await _unitOfWork.Genres.AddAsync(genre);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<GenreDto>(genre);
        }

        public async Task<GenreDto> UpdateAsync(Guid id, UpdateGenreDto updateDto)
        {
            var genre = await _unitOfWork.Genres.GetByIdAsync(id);
            if (genre == null)
                throw new ApplicationException("Genre not found");

            _mapper.Map(updateDto, genre);

            await _unitOfWork.Genres.UpdateAsync(genre);
            await _unitOfWork.CompleteAsync();

            return _mapper.Map<GenreDto>(genre);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _unitOfWork.Genres.DeleteAsync(id);
            await _unitOfWork.CompleteAsync();
        }
    }
}
