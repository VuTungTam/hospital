using Hospital.SharedKernel.Application.Enums;
using Hospital.SharedKernel.Runtime.Exceptions;

namespace Hospital.SharedKernel.Application.Models.Requests
{
    public class Pagination
    {
        private int _page = 0;
        private int _size = 20;

        public int Page
        {
            get
            {
                return _page;
            }

            set
            {
                if (value < 0)
                {
                    throw new BadRequestException("Page must be greater than or equal 0");
                }
                _page = value;
            }
        }

        public int Size
        {
            get
            {
                return _size;
            }

            set
            {
                if (value <= 0 || value > 1000)
                {
                    throw new BadRequestException("Size should be between 1 and 1000");
                }
                _size = value;
            }
        }

        public int Offset => _page * _size;

        public string Search { get; set; }

        public QueryType QueryType { get; set; } = QueryType.Contains;

        public object More { get; set; }

        //public Filter Filter { get; set; }

        public List<SortModel> Sorts { get; set; } = new List<SortModel>();

        public Pagination(int page, int size)
        {
            Page = page;
            Size = size;
        }

        public Pagination(int page, int size, string search, QueryType queryType) : this(page, size)
        {
            Search = search;
            QueryType = queryType;
        }

        public Pagination(int page, int size, string search) : this(page, size, search, QueryType.Contains)
        {
            Search = search;
        }

        public Pagination(int page, int size, string search, QueryType queryType, string orderBy, string orderByDescending) : this(page, size, search, queryType)
        {
            if (!string.IsNullOrEmpty(orderBy))
            {
                Sorts = Sorts ?? new List<SortModel>();
                Sorts.Add(new SortModel
                {
                    FieldName = orderBy,
                    SortAscending = true
                });
            }
            else if (!string.IsNullOrEmpty(orderByDescending))
            {
                Sorts = Sorts ?? new List<SortModel>();
                Sorts.Add(new SortModel
                {
                    FieldName = orderByDescending,
                    SortAscending = false
                });
            }
        }
    }
}
