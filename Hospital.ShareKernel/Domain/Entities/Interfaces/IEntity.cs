namespace Hospital.SharedKernel.Domain.Entities.Interfaces
{
    public interface IEntity
    {
        string GetTableName();
        object this[string propertyName] { get; set; }
    }
}
