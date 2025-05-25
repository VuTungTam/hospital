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

        public class ServiceTimeRuleValidator : BaseAbstractValidator<ServiceTimeRuleDto>
        {
            public ServiceTimeRuleValidator(IStringLocalizer<Resources> localizer) : base(localizer)
            {
                RuleFor(x => x.SlotDuration)
                    .GreaterThan(0).WithMessage(localizer["Thời lượng phải lớn hơn 0."]);

                RuleFor(x => x.MaxPatients)
                    .GreaterThan(0).WithMessage(localizer["Số lượng bệnh nhân tối đa phải lớn hơn 0."]);

                RuleFor(x => x.StartTime)
                    .NotEmpty().WithMessage(localizer["Giờ bắt đầu không được để trống."])
                    .LessThan(x => x.EndTime).WithMessage(localizer["Giờ bắt đầu phải nhỏ hơn giờ kết thúc."]);

                RuleFor(x => x.StartBreakTime)
                    .NotEmpty().WithMessage(localizer["Giờ bắt đầu nghỉ không được để trống."])
                    .GreaterThan(x => x.StartTime).WithMessage(localizer["Giờ bắt đầu nghỉ phải lớn hơn giờ bắt đầu."])
                    .LessThan(x => x.EndBreakTime).WithMessage(localizer["Giờ bắt đầu nghỉ phải nhỏ hơn giờ kết thúc nghỉ."]);

                RuleFor(x => x.EndBreakTime)
                    .NotEmpty().WithMessage(localizer["Giờ kết thúc nghỉ không được để trống."])
                    .GreaterThan(x => x.StartBreakTime).WithMessage(localizer["Giờ kết thúc nghỉ phải lớn hơn giờ bắt đầu nghỉ."]);

                RuleFor(x => x.EndTime)
                    .NotEmpty().WithMessage(localizer["Giờ kết thúc không được để trống."])
                    .GreaterThan(x => x.EndBreakTime).WithMessage(localizer["Giờ kết thúc phải lớn hơn giờ kết thúc nghỉ."]);

                RuleFor(x => x.DayOfWeek)
                    .IsInEnum().WithMessage(localizer["Ngày không hợp lệ. Vui lòng chọn một ngày trong tuần."]);
            }
        }
    }
}
