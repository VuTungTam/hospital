namespace Hospital.SharedKernel.Domain.Entities.Interfaces
{
    public interface ISoftDelete
    {
        DateTime? DeletedAt { get; set; }
    }
}
