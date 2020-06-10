using RedisInAction.Chapter6;
using StackExchange.Redis;

namespace RedisInAction
{
    class Program
    {
        static void Main(string[] args)
        {
            var autocomplete = new AddressBookAutocomplete(ConnectionMultiplexer.Connect("localhost"));

            autocomplete.JoinGuild("testGuild", "testUser").Wait();
            var items = autocomplete.FetchAutoComplete("testGuild", "testUser").GetAwaiter().GetResult();

            foreach (var item in items)
            {
                System.Console.WriteLine(item);
            }
        }
    }
}
