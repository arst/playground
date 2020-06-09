using StackExchange.Redis;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RedisInAction.Chapter6
{
    internal class SimpleAutocomplete
    {
        private readonly ConnectionMultiplexer connectionMultiplexer;

        public SimpleAutocomplete(ConnectionMultiplexer connectionMultiplexer)
        {
            this.connectionMultiplexer = connectionMultiplexer;
        }

        private async Task<List<string>> GetContacts(string userName)
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

        private async Task AddOrUpdateContact(string userName, string contactName)
        {
            var database = connectionMultiplexer.GetDatabase();
            var key = $"recent: {userName}";
            await Task.WhenAll(
                database.ListRemoveAsync(key, contactName),
                database.ListLeftPushAsync(key, contactName),
                database.ListTrimAsync(key, 0, 99));
        }

        private async Task RemoveContact(string userName, string contactName)
        {
            var database = connectionMultiplexer.GetDatabase();
            var key = $"recent: {userName}";
            await database.ListRemoveAsync(key, contactName, 0);
        }

        private async Task<List<string>> FetchAutoComplete(string userName, string prefix)
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
