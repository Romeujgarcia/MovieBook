using AutoMapper;
using MovieBookingSystem.Application.DTOs;
using MovieBookingSystem.Domain.Entities;
using System.Linq;

namespace MovieBookingSystem.Application.Common.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // User mappings
            CreateMap<User, UserDto>();
            CreateMap<RegisterUserDto, User>();
            CreateMap<UpdateUserDto, User>();

            // Movie mappings
            CreateMap<Movie, MovieDto>()
                .ForMember(dest => dest.Genres, opt => opt.MapFrom(src =>
                    src.MovieGenres.Select(mg => mg.Genre).ToList()));

            // Aqui está a modificação: adicionar o mapeamento específico para PosterImage
            CreateMap<CreateMovieDto, Movie>()
                .ForMember(dest => dest.PosterImage, opt => opt.MapFrom(src => src.PosterUrl));

            // Você também provavelmente deveria atualizar este mapeamento para o UpdateMovieDto
            CreateMap<UpdateMovieDto, Movie>()
                .ForMember(dest => dest.PosterImage, opt => opt.MapFrom(src => src.PosterUrl));

            // Genre mappings
            CreateMap<Genre, GenreDto>();
            CreateMap<CreateGenreDto, Genre>();
            CreateMap<UpdateGenreDto, Genre>();

            // Showtime mappings
            CreateMap<Showtime, ShowtimeDto>()
                .ForMember(dest => dest.MovieTitle, opt => opt.MapFrom(src => src.Movie.Title));
            CreateMap<CreateShowtimeDto, Showtime>();
            CreateMap<UpdateShowtimeDto, Showtime>();

            // Seat mappings
            CreateMap<Seat, SeatDto>();
            //CreateMap<CreateSeatDto, Seat>();

            // Reservation mappings
            CreateMap<Reservation, ReservationDto>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.MovieTitle, opt => opt.MapFrom(src => src.Showtime.Movie.Title))
                .ForMember(dest => dest.ShowtimeStart, opt => opt.MapFrom(src => src.Showtime.StartTime))
                .ForMember(dest => dest.Seats, opt => opt.MapFrom(src =>
                    src.ReservationSeats.Select(rs => rs.Seat).ToList()));
            CreateMap<Reservation, ReservationDto>()
    .ForMember(dest => dest.Seats, opt => opt.MapFrom(src => src.ReservationSeats.Select(rs => rs.Seat)));

            CreateMap<Seat, SeatDto>();

            CreateMap<CreateReservationDto, Reservation>();
        }
    }
}