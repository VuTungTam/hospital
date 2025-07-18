﻿using Hospital.Application.Dtos.Users;
using Hospital.Resource.Properties;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Dtos.Customers
{
    public class CustomerDto : BaseUserDto
    {
    }

    public class CustomerDtoValidator : BaseUserDtoDtoValidator<CustomerDto>
    {
        public CustomerDtoValidator(IStringLocalizer<Resources> localizer) : base(localizer)
        {
        }
    }
}
