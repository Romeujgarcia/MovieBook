using AutoMapper;
using MovieBookingSystem.Application.DTOs;
using MovieBookingSystem.Application.Interfaces;
using MovieBookingSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieBookingSystem.Application.Services
{
    public class SeatService : ISeatService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SeatService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<SeatDto> GetByIdAsync(Guid id)
        {
            var seat = await _unitOfWork.Seats.GetByIdAsync(id);
            return _mapper.Map<SeatDto>(seat);
        }

        public async Task<IEnumerable<SeatDto>> GetByShowtimeIdAsync(Guid showtimeId)
        {
            var seats = await _unitOfWork.Seats.GetByShowtimeIdAsync(showtimeId);
            return _mapper.Map<IEnumerable<SeatDto>>(seats);
        }

        public async Task<IEnumerable<SeatDto>> GetAvailableSeatsByShowtimeIdAsync(Guid showtimeId)
        {
            var seats = await _unitOfWork.Seats.GetAvailableSeatsByShowtimeIdAsync(showtimeId);
            return _mapper.Map<IEnumerable<SeatDto>>(seats);
        }
    }
}