namespace Hospital.SharedKernel.Infrastructure.Databases.Models
{
    public class QueryOption
    {
        public string[] Includes { get; set; }

        public bool IgnoreOwner { get; set; }

        public QueryOption()
        {
        }

        public QueryOption(string[] includes, bool ignoreOwner)
        {
            Includes = includes ?? new string[0];
            IgnoreOwner = ignoreOwner;
        }
    }
}
