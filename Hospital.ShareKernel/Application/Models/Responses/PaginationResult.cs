namespace Hospital.SharedKernel.Application.Models.Responses
{
    public interface IPaginationResult
    {
        int Total { get; set; }
    }

    public class PaginationResult<T> : IPaginationResult
    {
        public IEnumerable<T> Data { get; set; }

        public int Total { get; set; }

        public PaginationResult(IEnumerable<T> data, int total)
        {
            Data = data;
            Total = total;
        }
    }
}
