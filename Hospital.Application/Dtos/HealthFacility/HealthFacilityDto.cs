﻿using FluentValidation;
using Hospital.Application.Dtos.Images;
using Hospital.Application.Dtos.Specialties;
using Hospital.Domain.Enums;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.Validators;
using Hospital.SharedKernel.Configures.Models;
using Hospital.SharedKernel.Infrastructure.ExternalServices.Google.Maps.Utils;
using Hospital.SharedKernel.Infrastructure.Services.Emails.Utils;
using Hospital.SharedKernel.Libraries.ExtensionMethods;
using Hospital.SharedKernel.Libraries.Helpers;
using Microsoft.Extensions.Localization;
using Microsoft.OpenApi.Extensions;

namespace Hospital.Application.Dtos.HealthFacility
{
    public class HealthFacilityDto : BaseDto
    {
        public string NameVn { get; set; }

        public string NameEn { get; set; }

        public string DescriptionVn { get; set; }

        public string DescriptionEn { get; set; }

        public string SummaryVn { get; set; }

        public string SummaryEn { get; set; }

        public string Logo { get; set; }

        public string LogoUrl => CdnConfig.Get(Logo);

        public HealthFacilityStatus Status { get; set; }

        public string StatusText => Status.GetDescription();

        public string StatusTextEn => Status.GetDisplayName();

        public string Pid { get; set; }

        public string Pname { get; set; }

        public string Did { get; set; }

        public string Dname { get; set; }

        public string Wid { get; set; }

        public string Wname { get; set; }

        public string Address { get; set; }

        public string FullAddress => Address + ", " + Wname + ", " + Dname + ", " + Pname;

        public string MapURL { get; set; }

        public float StarPoint { get; set; }

        public int TotalStars { get; set; }

        public int TotalFeedback { get; set; }

        public string Slug { get; set; }

        public List<ImageDto> Images { get; set; }

        public List<string> ImageNames { get; set; } = new List<string>();

        public List<string> ImageUrls
        {
            get
            {
                if (Images == null) return new List<string>();
                return Images.Select(img => CdnConfig.Get(img.PublicId)).ToList();

            }
        }

        public List<SpecialtyDto> Specialties { get; set; }

        public List<string> SpecialtyIds { get; set; }

        public List<FacilityTypeDto> Types { get; set; }

        public List<string> TypeIds { get; set; }

        public string ListSpecialtyNameVns =>
        (Specialties != null && Specialties.Any())
            ? string.Join(", ", Specialties
                .Select(s => s.NameVn))
            : "Chưa có thông tin";

        public string ListSpecialtyNameEns =>
        (Specialties != null && Specialties.Any())
            ? string.Join(", ", Specialties
                .Select(s => s.NameEn))
            : "No data";

        public string ListTypeNameVns =>
        (Types != null && Types.Any())
            ? string.Join(", ", Types
                .Select(s => s.NameVn))
            : "Chưa có thông tin";


        public string ListTypeNameEns =>
        (Types != null && Types.Any())
            ? string.Join(", ", Types
                .Select(s => s.NameEn))
            : "No data";
    }
    public class HealthFacilityValidator : BaseAbstractValidator<HealthFacilityDto>
    {
        public HealthFacilityValidator(IStringLocalizer<Resources> localizer) : base(localizer)
        {
            RuleFor(x => x.Logo).Must(x => FileHelper.IsImageByFileName(x)).WithMessage(localizer["Utilities.ImageIsNotValid"]);
            RuleFor(x => x.NameVn).NotEmpty().WithMessage(localizer["HealthFacility.NameVnIsNotEmpty"]);
            RuleFor(x => x.NameEn).NotEmpty().WithMessage(localizer["HealthFacility.NameEnIsNotEmpty"]);
            RuleFor(x => x.Logo).NotEmpty().WithMessage(localizer["HealthFacility.LogoIsNotEmpty"]);
            RuleFor(x => x.DescriptionVn).NotEmpty().WithMessage(localizer["HealthFacility.DescriptionVnIsNotEmpty"]);
            RuleFor(x => x.DescriptionEn).NotEmpty().WithMessage(localizer["HealthFacility.DescriptionEnIsNotEmpty"]);
            RuleFor(x => x.SummaryVn).NotEmpty().WithMessage(localizer["HealthFacility.SummaryVnIsNotEmpty"]);
            RuleFor(x => x.SummaryEn).NotEmpty().WithMessage(localizer["HealthFacility.SummaryEnIsNotEmpty"]);
            RuleFor(x => x.MapURL)
                .Must(x => MapUtility.IsMapURL(x))
                .WithMessage(localizer["HealthFacility.InvalidMapUrl"]);
            RuleFor(x => x.SpecialtyIds)
                .NotNull().WithMessage(localizer["HealthFacility.SpecialtyIsRequired"])
                .Must(x => x.Any()).WithMessage(localizer["HealthFacility.SpecialtyIsRequired"]);
            RuleFor(x => x.TypeIds)
                .NotNull().WithMessage(localizer["HealthFacility.TypeIsRequired"])
                .Must(x => x.Any()).WithMessage(localizer["HealthFacility.TypeIsRequired"]);
        }
    }
}
