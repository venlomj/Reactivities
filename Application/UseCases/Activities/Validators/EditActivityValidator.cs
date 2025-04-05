using Application.UseCases.Activities.Commands;
using Application.UseCases.Activities.DTOs;
using FluentValidation;

namespace Application.UseCases.Activities.Validators
{
    public class EditActivityValidator : BaseActivityValidator<EditActivity.Command, EditActivityDto>
    {
        public EditActivityValidator() : base(x => x.ActivityDto)
        {
            RuleFor(x => x.ActivityDto.Id)
                .NotEmpty().WithMessage("Id is required");
        }
    }
}
