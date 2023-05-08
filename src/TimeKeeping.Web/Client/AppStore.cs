using TimeKeeping.Web.Shared.HttpModel;

namespace TimeKeeping.Web.Client
{
    public class AppStore
    {
        public Task Upsert(Entry entry)
        {
            return Task.Delay(11);
        }

        public Task RemoveEntry(Entry entry)
        {
            return Task.Delay(11);
        }
    }
}
