namespace Hospital.SharedKernel.Application.Repositories.Models
{
    public class QueryOption
    {
        public IEnumerable<string> Includes { get; set; }

        public bool AsNoTracking { get; set; }

        public bool Guard { get; set; }

        public QueryOption(IEnumerable<string> includes, bool asNoTracking, bool guard)
        {
            Includes = includes;
            AsNoTracking = asNoTracking;
            Guard = guard;
        }

        public QueryOption(IEnumerable<string> includes) : this(includes, true, true)
        {
        }

        public QueryOption() : this(null)
        {
        }
    }
}
