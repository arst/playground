using StackExchange.Redis;
using System;
using System.Threading.Tasks;

namespace RedisInAction.Chapter6
{
    internal sealed class CountigSemaphor
    {
        private readonly ConnectionMultiplexer connectionMultiplexer;
        private readonly int limit;
        private readonly LockWithTimeout singleLock;

        internal CountigSemaphor(ConnectionMultiplexer connectionMultiplexer, int limit)
        {
            this.connectionMultiplexer = connectionMultiplexer;
            this.limit = limit;
            this.singleLock = new LockWithTimeout(connectionMultiplexer);
        }

        internal async Task<string> AcquireFairSemaphor(string semaphoreName, TimeSpan timeout)
        {
            var acquired = singleLock.TryAcquire(semaphoreName, timeout, timeout, out string identifier);

            if (!acquired)
            {
                return null;
            }

            try
            {
                return await AcquireFairSemaphorInternal(semaphoreName, timeout);
            }
            finally
            {
                singleLock.TryRelease(semaphoreName, identifier);
            }
        }

        private async Task<string> AcquireFairSemaphorInternal(string semaphoreName, TimeSpan timeout)
        {
            var identifier = Guid.NewGuid().ToString();
            var semaphoreOwnerSetName = semaphoreName + ":owner";
            var semaphoreCounterName = semaphoreName + ":counter";

            var now = DateTimeOffset.UtcNow;

            var database = connectionMultiplexer.GetDatabase();

            var cleanupTransaction = database.CreateTransaction();

            _ = cleanupTransaction.SortedSetRemoveRangeByScoreAsync(semaphoreName, Double.NegativeInfinity, now.ToUnixTimeSeconds() - timeout.TotalSeconds);
             
            _ = cleanupTransaction.SortedSetCombineAndStoreAsync(SetOperation.Intersect, semaphoreOwnerSetName, keys: new RedisKey[] { semaphoreOwnerSetName, semaphoreName }, weights: new[] { 1.0, 0.0 });

            var counterTask = cleanupTransaction.StringIncrementAsync(semaphoreCounterName);

            await cleanupTransaction.ExecuteAsync();

            var counter = await counterTask;

            var acquireTransaction = database.CreateTransaction();

            _ = acquireTransaction.SortedSetAddAsync(semaphoreName, identifier, now.ToUnixTimeSeconds());
            _ = acquireTransaction.SortedSetAddAsync(semaphoreOwnerSetName, identifier, counter);

            var rankTask = acquireTransaction.SortedSetRankAsync(semaphoreOwnerSetName, identifier);

            var executed = await acquireTransaction.ExecuteAsync();

            var rank = await rankTask;

            if (rank < limit && executed)
            {
                return identifier;
            }

            await database.SortedSetRemoveAsync(semaphoreName, identifier);
            await database.SortedSetRemoveAsync(semaphoreOwnerSetName, identifier);

            return null;
        }

        internal async Task<bool> ReleaseSemaphore(string semaphoreName, string identifier)
        {
            var database = connectionMultiplexer.GetDatabase();

            var result = await database.SortedSetRemoveAsync(semaphoreName, identifier);
            await database.SortedSetRemoveAsync(semaphoreName + ":owner", identifier);

            return result;
        }

        internal async Task<bool> Refresh(string semaphoreName, string identifier)
        {
            var database = connectionMultiplexer.GetDatabase();

            if (database.SortedSetAdd(semaphoreName, identifier, DateTimeOffset.UtcNow.ToUnixTimeSeconds()))
            {
                await ReleaseSemaphore(semaphoreName, identifier);

                return false;
            }

            return true;
        }
    }
}
