using FluentValidation;
using MovieBookingSystem.Application.DTOs;
using System;

namespace MovieBookingSystem.Application.Validators
{
    public class CreateMovieDtoValidator : AbstractValidator<CreateMovieDto>
    {
        public CreateMovieDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(200).WithMessage("Title cannot exceed 200 characters");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required");

            RuleFor(x => x.DurationMinutes)
                .GreaterThan(0).WithMessage("Duration must be greater than 0 minutes");

            RuleFor(x => x.Director)
                .NotEmpty().WithMessage("Director is required")
                .MaximumLength(100).WithMessage("Director name cannot exceed 100 characters");

            RuleFor(x => x.ReleaseDate)
                .NotEmpty().WithMessage("Release date is required");

            RuleFor(x => x.GenreIds)
                .NotEmpty().WithMessage("At least one genre must be selected");
        }
    }

    public class UpdateMovieDtoValidator : AbstractValidator<UpdateMovieDto>
    {
        public UpdateMovieDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(200).WithMessage("Title cannot exceed 200 characters");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required");

            RuleFor(x => x.DurationMinutes)
                .GreaterThan(0).WithMessage("Duration must be greater than 0 minutes");

            RuleFor(x => x.Director)
                .NotEmpty().WithMessage("Director is required")
                .MaximumLength(100).WithMessage("Director name cannot exceed 100 characters");

            RuleFor(x => x.ReleaseDate)
                .NotEmpty().WithMessage("Release date is required");
        }
    }
}
