using Application.UseCases.Activities.Commands;
using Application.UseCases.Activities.DTOs;
using FluentValidation;

namespace Application.UseCases.Activities.Validators
{
    public class CreateActivityValidator
        : BaseActivityValidator<CreateActivity.Command, CreateActivityDto>
    {
        public CreateActivityValidator() : base(x => x.ActivityDto)
        {

        }
    }
}
