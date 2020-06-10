using StackExchange.Redis;
using System;
using System.Threading;

namespace RedisInAction.Chapter6
{
    internal sealed class Lock
    {
        private readonly ConnectionMultiplexer connectionMultiplexer;

        internal Lock(ConnectionMultiplexer connectionMultiplexer)
        {
            this.connectionMultiplexer = connectionMultiplexer;
        }

        internal bool TryAcquire(string lockName, TimeSpan timeout, out string lockIdentifier)
        {
            var identifier = Guid.NewGuid().ToString();
            lockIdentifier = null;

            var end = DateTime.UtcNow + timeout;

            var database = connectionMultiplexer.GetDatabase();

            while (DateTime.UtcNow < end)
            {
                if (database.StringSet("lock:" + lockName, identifier, null, When.NotExists))
                {
                    lockIdentifier = identifier;
                    return true;
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
