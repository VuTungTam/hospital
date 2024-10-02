using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Libraries.Attributes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace Hospital.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ApiBaseController : ControllerBase
    {
        protected readonly IMediator _mediator;

        public ApiBaseController(IMediator mediator)
        {
            _mediator = mediator;
        }

        protected IActionResult GetFilterable<TEntity>() where TEntity : BaseEntity
        {
            var properties = typeof(TEntity).GetProperties().Where(p => p.GetIndexParameters().Length == 0);
            var result = new List<object>();

            foreach (var property in properties)
            {
                if (Attribute.IsDefined(property, typeof(FilterableAttribute)))
                {
                    result.Add(new
                    {
                        Key = property.Name,
                        Text = ((FilterableAttribute)property.GetCustomAttribute(typeof(FilterableAttribute))).DisplayName,
                    });
                }
            }
            return Ok(new SimpleDataResult { Data = result });
        }
    }
}
