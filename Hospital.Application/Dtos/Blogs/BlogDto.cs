namespace Hospital.Application.Dtos.Blogs
{
    public class BlogDto : BaseDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string ImageUrl { get; set; }
    }
}
