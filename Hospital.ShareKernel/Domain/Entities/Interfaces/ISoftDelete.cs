namespace Hospital.SharedKernel.Domain.Entities.Interfaces
{
    public interface ISoftDelete
    {
        DateTime? Deleted { get; set; }
    }
}
