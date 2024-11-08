using Hospital.Domain.Entities.Newses;
using Hospital.SharedKernel.Application.Repositories.Interface;

namespace Hospital.Application.Repositories.Interfaces.Newes
{
    public interface INewsWriteRepository : IWriteRepository<News>
    {
    }
}
