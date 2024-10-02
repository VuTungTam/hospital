using Hospital.Domain.Entities.Blogs;
using Hospital.SharedKernel.Application.Repositories.Interface;

namespace Hospital.Application.Repositories.Interfaces.Blogs
{
    public interface IBlogReadRepository : IReadRepository<Blog>
    {
    }
}
