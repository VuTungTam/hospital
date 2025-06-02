using FluentValidation;
using Hospital.Domain.Entities.HealthServices;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Validators;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Dtos.ServiceTimeRules
{
    public class ServiceTimeRuleDto : BaseDto
    {
        public string ServiceId { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan StartBreakTime { get; set; }

        public TimeSpan EndBreakTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public int SlotDuration { get; set; }

        public int MaxPatients { get; set; }

        public DayOfWeek DayOfWeek { get; set; }

        public bool AllowWalkin { get; set; }

        public class ServiceTimeRuleValidator : BaseAbstractValidator<ServiceTimeRuleDto>
        {
            public ServiceTimeRuleValidator(IStringLocalizer<Resources> localizer) : base(localizer)
            {
                RuleFor(x => x.SlotDuration)
                    .GreaterThan(0).WithMessage(localizer["ServiceTimeRule.SlotDurationMustBeGreaterThanZero"]);

                RuleFor(x => x.MaxPatients)
                    .GreaterThan(0).WithMessage(localizer["ServiceTimeRule.MaxPatientsMustBeGreaterThanZero"]);

                RuleFor(x => x.StartTime)
                    .NotEmpty().WithMessage(localizer["ServiceTimeRule.StartTimeIsRequired"])
                    .LessThan(x => x.EndTime).WithMessage(localizer["ServiceTimeRule.StartTimeMustBeLessThanEndTime"]);

                RuleFor(x => x.StartBreakTime)
                    .NotEmpty().WithMessage(localizer["ServiceTimeRule.StartBreakTimeIsRequired"])
                    .GreaterThan(x => x.StartTime).WithMessage(localizer["ServiceTimeRule.StartBreakTimeMustBeGreaterThanStartTime"])
                    .LessThan(x => x.EndBreakTime).WithMessage(localizer["ServiceTimeRule.StartBreakTimeMustBeLessThanEndBreakTime"]);

                RuleFor(x => x.EndBreakTime)
                    .NotEmpty().WithMessage(localizer["ServiceTimeRule.EndBreakTimeIsRequired"])
                    .GreaterThan(x => x.StartBreakTime).WithMessage(localizer["ServiceTimeRule.EndBreakTimeMustBeGreaterThanStartBreakTime"]);

                RuleFor(x => x.EndTime)
                    .NotEmpty().WithMessage(localizer["ServiceTimeRule.EndTimeIsRequired"])
                    .GreaterThan(x => x.EndBreakTime).WithMessage(localizer["ServiceTimeRule.EndTimeMustBeGreaterThanEndBreakTime"]);

                RuleFor(x => x.DayOfWeek)
                    .IsInEnum().WithMessage(localizer["ServiceTimeRule.DayOfWeekIsInvalid"]);
            }
        }
    }
}
