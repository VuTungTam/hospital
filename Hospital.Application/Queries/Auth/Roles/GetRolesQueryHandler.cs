using AutoMapper;
using Hospital.Application.Dtos.Auth;
using Hospital.Application.Repositories.Interfaces.Auth.Roles;
using Hospital.Domain.Constants;
using Hospital.Resource.Properties;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Services.Auth.Interfaces;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Hospital.Application.Queries.Auth.Roles
{
    public class GetRolesQueryHandler : BaseQueryHandler, IRequestHandler<GetRolesQuery, List<RoleDto>>
    {
        private readonly IRoleReadRepository _roleReadRepository;
        public GetRolesQueryHandler(
            IAuthService authService, 
            IMapper mapper, 
            IStringLocalizer<Resources> localizer,
            IRoleReadRepository roleReadRepository
            ) : base(authService, mapper, localizer)
        {
            _roleReadRepository = roleReadRepository;
        }

        public async Task<List<RoleDto>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
        {
            var roles = await _roleReadRepository.GetAsync(cancellationToken: cancellationToken);

            roles = roles.Where(x => x.Code != RoleCodeConstant.CUSTOMER && x.Code != RoleCodeConstant.DOCTOR).ToList();

            return _mapper.Map<List<RoleDto>>(roles);
        }
    }
}
