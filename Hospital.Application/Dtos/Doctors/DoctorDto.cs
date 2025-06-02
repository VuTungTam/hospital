using System.Data;
using FluentValidation;
using Hospital.Application.Dtos.Specialties;
using Hospital.Application.Dtos.Users;
using Hospital.Domain.Enums;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Validators;
using Hospital.SharedKernel.Libraries.ExtensionMethods;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Dtos.Doctors
{
    public class DoctorDto : BaseUserDto
    {
        public DoctorTitle DoctorTitle { get; set; }

        public DoctorDegree DoctorDegree { get; set; }

        public DoctorRank DoctorRank { get; set; }

        public string DoctorTitleStr { get; set; }

        public string DoctorDegreeStr { get; set; }

        public string DoctorRankStr { get; set; }

        public string ExpertiseVn { get; set; }

        public string ExpertiseEn { get; set; }

        public string ProfessionalLevel { get; set; }

        public List<SpecialtyDto> Specialties { get; set; } = new List<SpecialtyDto>();

        public List<string> SpecialtyIds { get; set; } = new List<string>();

        public string ListSpecialtyNameVns => Specialties != null && Specialties.Any()
        ? string.Join(", ", Specialties
            .Select(s => s.NameVn))
        : "Chưa có thông tin";

    }
    public class DoctorValidator : BaseAbstractValidator<DoctorDto>
    {
        public DoctorValidator(IStringLocalizer<Resources> localizer) : base(localizer)
        {
            RuleFor(x => x.DoctorTitle)
                .IsInEnum().WithMessage(localizer["Doctor.TitleIsRequired"]);

            RuleFor(x => x.DoctorDegree)
                .IsInEnum().WithMessage(localizer["Doctor.DegreeIsRequired"]);

            RuleFor(x => x.DoctorRank)
                .IsInEnum().WithMessage(localizer["Doctor.RankIsRequired"]);

            RuleFor(x => x.ExpertiseVn)
                .NotEmpty().WithMessage(localizer["Doctor.ExpertiseVnIsRequired"]);

            RuleFor(x => x.ExpertiseEn)
                .NotEmpty().WithMessage(localizer["Doctor.ExpertiseEnIsRequired"]);

            RuleFor(x => x.SpecialtyIds)
                .NotEmpty().WithMessage(localizer["Doctor.SpecialtyIsRequired"])
                .Must(list => list != null && list.All(id => !string.IsNullOrWhiteSpace(id)))
                .WithMessage(localizer["Doctor.SpecialtyIdInvalid"]);
        }
    }
}

