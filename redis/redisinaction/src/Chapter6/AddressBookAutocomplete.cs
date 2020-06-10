using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RedisInAction.Chapter6
{
    internal sealed class AddressBookAutocomplete
    {

        private readonly ConnectionMultiplexer connectionMultiplexer;

        internal AddressBookAutocomplete(ConnectionMultiplexer connectionMultiplexer)
        {
            this.connectionMultiplexer = connectionMultiplexer;
        }

        private (string RangeStart, string RangeEnd) FindPrefixRange(string prefix)
        {
            var validCharacters = "`abcdefghijklmnopqrstuvwxyz{";

            var lastCharacterPosition = validCharacters.IndexOf(prefix.Last());

            var suffix = validCharacters[(lastCharacterPosition == 0 ? 1 : lastCharacterPosition) - 1];

            return (prefix[0..^1] + suffix + '{', prefix + '{');
        }

        public async Task<IReadOnlyCollection<string>> FetchAutoComplete(string guild, string prefix)
        {
            var (rangeStart, rangeEnd) = FindPrefixRange(prefix);

            var identifier = Guid.NewGuid();

            rangeStart += identifier;

            rangeEnd += identifier;

            var sortedSetName = "members: " + guild;

            var database = connectionMultiplexer.GetDatabase();

            database.SortedSetAdd(sortedSetName, rangeStart, 0);
            database.SortedSetAdd(sortedSetName, rangeEnd, 0);
            var commited = false;
            RedisValue[] items = null;

            while (!commited)
            {
                var startRank = await database.SortedSetRankAsync(sortedSetName, rangeStart);
                var endRank = await database.SortedSetRankAsync(sortedSetName, rangeEnd);
                var endRange = Math.Min(startRank.Value + 9, endRank.Value - 2);

                var transaction = database.CreateTransaction();

                var startRankPending = database.SortedSetRankAsync(sortedSetName, rangeStart);
                var endRankPending = database.SortedSetRankAsync(sortedSetName, rangeEnd);
                
                var removeRangeStartPending = transaction.SortedSetRemoveAsync(sortedSetName, rangeStart);
                var removeRangeEndPending = transaction.SortedSetRemoveAsync(sortedSetName, rangeEnd);

                var itemsPending = transaction.SortedSetRangeByRankAsync(sortedSetName, startRank.Value, endRange);

                commited = await transaction.ExecuteAsync();
                var transactionalStartRank = await startRankPending;
                var transactionalEndRank = await endRankPending;

                if (transactionalStartRank != startRank || transactionalEndRank != endRank)
                {
                    commited = false;
                    continue;
                }

                items = await itemsPending;
            }

            return items
                .Select(arg => arg.ToString())
                .Where(arg => !arg.Contains("{"))
                .ToList()
                .AsReadOnly();
        }

        public async Task JoinGuild(string guildName, string userName)
        {
            var database = connectionMultiplexer.GetDatabase();

            await database.SortedSetAddAsync("members: " + guildName, userName, 0);
        }

        public async Task LeaveGuild(string guildName, string userName)
        {
            var database = connectionMultiplexer.GetDatabase();

            await database.SortedSetRemoveAsync("members: " + guildName, userName);
        }
    }
}
