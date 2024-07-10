namespace Hospital.SharedKernel.Application.Models.Responses
{
    public class PagingResult<T>
    {
        public IEnumerable<T> Data { get; set; }

        public int Total { get; set; }

        public PagingResult(IEnumerable<T> data, int total)
        {
            Data = data;
            Total = total;
        }
    }
}
