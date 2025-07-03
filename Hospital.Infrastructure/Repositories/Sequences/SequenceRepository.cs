using Hospital.Domain.Constants;
using Hospital.Infrastructure.EFConfigurations;
using Hospital.SharedKernel.Caching.In_Memory;
using Hospital.SharedKernel.Infrastructure.Caching.Models;
using Hospital.SharedKernel.Infrastructure.Redis;
using Hospital.SharedKernel.Infrastructure.Repositories.Sequences.Entities;
using Hospital.SharedKernel.Infrastructure.Repositories.Sequences.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Hospital.Application.Repositories.Interfaces.Sequences
{
    public class SequenceRepository : ISequenceRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IRedisCache _redisCache;

        public SequenceRepository(AppDbContext dbContext, IRedisCache redisCache)
        {
            _dbContext = dbContext;
            _redisCache = redisCache;
        }

        public async Task<Sequence> GetSequenceAsync(string table, CancellationToken cancellationToken)
        {
            var cacheEntry = CacheManager.GetSequenceCacheEntry(table);
            var sequence = await _redisCache.GetAsync<Sequence>(cacheEntry.Key, cancellationToken: cancellationToken);
            if (sequence == null)
            {
                sequence = await _dbContext.Sequences.AsNoTracking().FirstOrDefaultAsync(x => x.Table == table, cancellationToken);
                await _redisCache.SetAsync(cacheEntry.Key, sequence, TimeSpan.FromSeconds(AppCacheTime.Sequence), cancellationToken);
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
            string sql;
            if (table == "admin")
            {
                sql = $"UPDATE {new Sequence().GetTableName()} SET Value = Value + 1 WHERE [{nameof(Sequence.Table)}] = 'admin'";
            }
            else
            {
                sql = $"UPDATE {new Sequence().GetTableName()} SET Value = Value + 1 WHERE [{nameof(Sequence.Table)}] = {{0}}";
            }

            await _dbContext.Database.ExecuteSqlRawAsync(sql, table);

            var cacheEntry = CacheManager.GetSequenceCacheEntry(table);
            await _redisCache.RemoveAsync(cacheEntry.Key, cancellationToken);
        }

        public void UpdateSequence(Sequence sequence)
        {
            _dbContext.Sequences.Update(sequence);
        }
    }
}
