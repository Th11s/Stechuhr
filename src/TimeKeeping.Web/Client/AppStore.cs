using TimeKeeping.Web.Client.Clients;
using TimeKeeping.Web.Shared.HttpModel;

namespace TimeKeeping.Web.Client
{
    public class AppStore
    {
        private readonly TimeKeepingClient _client;

        public AppStore(TimeKeepingClient client)
        {
            _client = client;
        }

        public async Task Upsert(Entry entry)
        {
            await _client.UpsertEntryAsync(entry);
        }

        public async Task RemoveEntry(Entry entry)
        {
            await _client.DeleteEntryAsync(entry.Uuid);
        }

        public async Task<List<Entry>> GetToday()
        {
            var date = DateTimeOffset.Now;
            return (await _client.GetByDateAsync(date)).ToList();
        }
    }
}
