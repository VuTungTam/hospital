namespace Hospital.SharedKernel.Libraries.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FilterableAttribute : Attribute
    {
        public readonly string DisplayName;

        public FilterableAttribute(string displayName)
        {
            DisplayName = displayName;
        }
    }
}
