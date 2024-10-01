using Hospital.SharedKernel.Infrastructure.Repositories.Sequences.Entities;

namespace Hospital.SharedKernel.Infrastructure.Repositories.Sequences.Interfaces
{
    public interface ISequenceRepository
    {
        Task<Sequence> GetSequenceAsync(string table, CancellationToken cancellationToken);

        Task AddSequenceAsync(Sequence sequence, CancellationToken cancellationToken);

        Task IncreaseValueAsync(string table, CancellationToken cancellationToken);

        void UpdateSequence(Sequence sequence);
    }
}
