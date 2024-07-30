namespace Hospital.SharedKernel.Domain.Entities.Interfaces
{
    public interface IDeletedBy
    {
        long? DeletedBy { get; set; }
    }
}
