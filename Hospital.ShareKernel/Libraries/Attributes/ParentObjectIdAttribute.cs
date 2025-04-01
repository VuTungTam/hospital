namespace Hospital.SharedKernel.Libraries.Attributes
{
    [AttributeUsage(AttributeTargets.Field)]
    public class ParentObjectIdAttribute : Attribute
    {
        public readonly int ParentId;

        public ParentObjectIdAttribute(int parentId)
        {
            ParentId = parentId;
        }
    }
}
