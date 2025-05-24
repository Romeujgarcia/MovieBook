using FluentValidation;
using MovieBookingSystem.Application.DTOs;
using System;

namespace MovieBookingSystem.Application.Validators
{
    public class CreateReservationDtoValidator : AbstractValidator<CreateReservationDto>
    {
        public CreateReservationDtoValidator()
        {
            RuleFor(x => x.ShowtimeId)
                .NotEmpty().WithMessage("Showtime ID is required");

            RuleFor(x => x.SeatIds)
                .NotEmpty().WithMessage("At least one seat must be selected");
        }
    }
}
