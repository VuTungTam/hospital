using FluentValidation;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Validators;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Models
{
    public class UpdateProfileModel
    {
        public string Name { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public DateTime? Dob { get; set; }

        public int Pid { get; set; }

        public int Did { get; set; }

        public int Wid { get; set; }

        public string Address { get; set; }
    }

    public class UpdateProfileModelValidator : BaseAbstractValidator<UpdateProfileModel>
    {
        public UpdateProfileModelValidator(IStringLocalizer<Resources> localizer) : base(localizer)
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage(localizer["Authentication.FullNameMustNotBeEmpty"]);
        }
    }
}
