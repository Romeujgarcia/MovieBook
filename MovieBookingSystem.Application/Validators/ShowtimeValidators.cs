using FluentValidation;
using MovieBookingSystem.Application.DTOs;
using System;

namespace MovieBookingSystem.Application.Validators
{
    public class CreateShowtimeDtoValidator : AbstractValidator<CreateShowtimeDto>
    {
        public CreateShowtimeDtoValidator()
        {
            RuleFor(x => x.MovieId)
                .NotEmpty().WithMessage("Movie ID is required");

            RuleFor(x => x.StartTime)
                .NotEmpty().WithMessage("Start time is required")
                .GreaterThan(DateTime.UtcNow).WithMessage("Start time must be in the future");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0");

            RuleFor(x => x.Theater)
                .NotEmpty().WithMessage("Theater is required")
                .MaximumLength(50).WithMessage("Theater name cannot exceed 50 characters");

            RuleFor(x => x.TotalRows)
                .GreaterThan(0).WithMessage("Total rows must be greater than 0")
                .LessThanOrEqualTo(26).WithMessage("Total rows cannot exceed 26");

            RuleFor(x => x.SeatsPerRow)
                .GreaterThan(0).WithMessage("Seats per row must be greater than 0")
                .LessThanOrEqualTo(30).WithMessage("Seats per row cannot exceed 30");
        }
    }

    public class UpdateShowtimeDtoValidator : AbstractValidator<UpdateShowtimeDto>
    {
        public UpdateShowtimeDtoValidator()
        {
            RuleFor(x => x.StartTime)
                .NotEmpty().WithMessage("Start time is required")
                .GreaterThan(DateTime.UtcNow).WithMessage("Start time must be in the future");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0");

            RuleFor(x => x.Theater)
                .NotEmpty().WithMessage("Theater is required")
                .MaximumLength(50).WithMessage("Theater name cannot exceed 50 characters");
        }
    }
}
