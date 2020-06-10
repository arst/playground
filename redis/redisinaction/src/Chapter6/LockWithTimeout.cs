using StackExchange.Redis;
using System;
using System.IO;
using System.Threading;

namespace RedisInAction.Chapter6
{
    internal sealed class LockWithTimeout
    {
        private readonly ConnectionMultiplexer connectionMultiplexer;

        internal LockWithTimeout(ConnectionMultiplexer connectionMultiplexer)
        {
            this.connectionMultiplexer = connectionMultiplexer;
        }

        internal bool TryAcquire(string lockName, TimeSpan acquireTimeout, TimeSpan lockTimeout, out string lockIdentifier)
        {
            var identifier = Guid.NewGuid().ToString();
            lockIdentifier = null;

            var end = DateTime.UtcNow + acquireTimeout;

            var database = connectionMultiplexer.GetDatabase();

            while (DateTime.UtcNow < end)
            {
                string lockKey = "lock:" + lockName;
                if (database.StringSet(lockKey, identifier, null, When.NotExists))
                {
                    database.KeyExpire(lockKey, lockTimeout);
                    lockIdentifier = identifier;
                    return true;
                }
                else if (!database.KeyTimeToLive(lockKey).HasValue)
                {
                    database.KeyExpire(lockKey, lockTimeout);
                }

                Thread.Sleep(TimeSpan.FromMilliseconds(100));
            }

            return false;
        }

        internal bool TryRelease(string lockName, string identifier)
        {
            while (true)
            {
                var database = connectionMultiplexer.GetDatabase();
                var lockKey = "lock:" + lockName;
                var lockIdentifier = database.StringGet(lockKey);

                if (lockIdentifier != identifier)
                {
                    break;
                }

                var transaction = database.CreateTransaction();
                transaction.AddCondition(Condition.StringEqual(lockKey, identifier));
                transaction.KeyDeleteAsync(lockKey);
                var executed = transaction.Execute();

                if (executed)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
