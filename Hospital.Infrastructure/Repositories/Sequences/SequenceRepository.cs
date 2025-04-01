using Hospital.Domain.Constants;
using Hospital.Infrastructure.EFConfigurations;
using Hospital.SharedKernel.Caching.In_Memory;
using Hospital.SharedKernel.Infrastructure.Caching.Models;
using Hospital.SharedKernel.Infrastructure.Repositories.Sequences.Entities;
using Hospital.SharedKernel.Infrastructure.Repositories.Sequences.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Hospital.Application.Repositories.Interfaces.Sequences
{
    public class SequenceRepository : ISequenceRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IMemoryCache _memoryCache;

        public SequenceRepository(AppDbContext dbContext, IMemoryCache memoryCache)
        {
            _dbContext = dbContext;
            _memoryCache = memoryCache;
        }

        public async Task<Sequence> GetSequenceAsync(string table, CancellationToken cancellationToken)
        {
            var cacheEntry = CacheManager.GetSequenceCacheEntry(table);
            var sequence = JsonConvert.DeserializeObject<Sequence>(JsonConvert.SerializeObject(_memoryCache.Get(cacheEntry.Key)));
            if (sequence == null)
            {
                sequence = await _dbContext.Sequences.AsNoTracking().FirstOrDefaultAsync(x => x.Table == table, cancellationToken);
                _memoryCache.Set(cacheEntry.Key, sequence, TimeSpan.FromSeconds(AppCacheTime.Sequence));
            }

            return sequence;
        }

        public async Task AddSequenceAsync(Sequence sequence, CancellationToken cancellationToken)
        {
            await _dbContext.Sequences.AddAsync(sequence, cancellationToken);
            await _dbContext.CommitAsync(cancellationToken: cancellationToken);
        }

        public async Task IncreaseValueAsync(string table, CancellationToken cancellationToken)
        {
            var sql = $"UPDATE {new Sequence().GetTableName()} SET Value = Value + 1 WHERE [{nameof(Sequence.Table)}] = {{0}}";

            await _dbContext.Database.ExecuteSqlRawAsync(sql, table);

            var cacheEntry = CacheManager.GetSequenceCacheEntry(table);
            _memoryCache.Remove(cacheEntry.Key);
        }

        public void UpdateSequence(Sequence sequence)
        {
            _dbContext.Sequences.Update(sequence);
        }
    }
}
