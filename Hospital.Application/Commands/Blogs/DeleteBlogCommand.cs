using Hospital.SharedKernel.Application.CQRS.Commands.Base;
namespace Hospital.Application.Commands.Blogs
{
    public class DeleteBlogCommand : BaseCommand
    {
        public DeleteBlogCommand(List<long> ids)
        {
            Ids = ids;
        }
        public List<long> Ids { get; set; }
    }
}
