namespace Hospital.SharedKernel.Domain.Entities.Interfaces
{
    public interface IOwnedEntity
    {
        long OwnerId { get; set; }
    }
}
