namespace Hospital.SharedKernel.Domain.Entities.Interfaces
{
    public interface IDeleteBy
    {
        long? DeletedBy { get; set; }
    }
}
