using StackExchange.Redis;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisInAction.Chapter6
{
    internal sealed class SimpleAutocomplete
    {
        private readonly ConnectionMultiplexer connectionMultiplexer;

        internal SimpleAutocomplete(ConnectionMultiplexer connectionMultiplexer)
        {
            this.connectionMultiplexer = connectionMultiplexer;
        }

        internal async Task<IReadOnlyCollection<string>> GetContacts(string userName)
        {
            var database = connectionMultiplexer.GetDatabase();
            var key = $"recent: {userName}";
            var values = await database.ListRangeAsync(key);
            var result = new List<string>();

            foreach (var value in values)
            {
                result.Add(value.ToString());
            }

            return result;
        }

        internal async Task AddOrUpdateContact(string userName, string contactName)
        {
            var database = connectionMultiplexer.GetDatabase();
            var key = $"recent: {userName}";
            await Task.WhenAll(
                database.ListRemoveAsync(key, contactName),
                database.ListLeftPushAsync(key, contactName),
                database.ListTrimAsync(key, 0, 99));
        }

        internal async Task RemoveContact(string userName, string contactName)
        {
            var database = connectionMultiplexer.GetDatabase();
            var key = $"recent: {userName}";
            await database.ListRemoveAsync(key, contactName, 0);
        }

        internal async Task<IReadOnlyCollection<string>> FetchAutoComplete(string userName, string prefix)
        {
            var database = connectionMultiplexer.GetDatabase();
            var key = $"recent: {userName}";

            var contacts = await database.ListRangeAsync(key);

            return contacts
                .Where(arg => arg.StartsWith(new RedisValue(prefix)))
                .Select(arg => arg.ToString())
                .ToList();
        }
    }
}
