using Application.UseCases.Profiles.Commands;
using FluentValidation;

namespace Application.UseCases.Profiles.Validators
{
    public class EditProfileValidator : AbstractValidator<EditProfile.Command>
    {
        public EditProfileValidator()
        {
            RuleFor(x => x.DisplayName).NotEmpty();
        }
    }
}
