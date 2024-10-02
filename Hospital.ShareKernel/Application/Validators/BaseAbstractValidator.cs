using FluentValidation;
using Microsoft.Extensions.Localization;
using Hospital.Resource.Properties;

namespace Hospital.SharedKernel.Application.Validators
{
    public class BaseAbstractValidator<T> : AbstractValidator<T>
    {
        public BaseAbstractValidator(IStringLocalizer<Resources> localizer)
        {
            CascadeMode = CascadeMode.Stop;
        }
    }
}
