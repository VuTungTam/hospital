namespace Hospital.SharedKernel.Domain.Models.Auths
{
    public class ActionWithExcludeValue
    {
        public long Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string NameEn { get; set; }

        public string Description { get; set; }

        public int Exponent { get; set; }

        public bool IsInternal { get; set; }

        public bool IsExclude { get; set; }
    }
}
