using Hospital.Application.Repositories.Interfaces.Blogs;
using Hospital.Domain.Entities.Blogs;
using Hospital.Infra.Repositories;

namespace Hospital.Infrastructure.Repositories.Blogs
{
    public class BlogWriteRepository : WriteRepository<Blog>, IBlogWriteRepository
    {
        public BlogWriteRepository(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }
    }
}
