using Th11s.TimeKeeping.SharedModel.Requests;
using TimeKeeping.Web.Client.Clients;
using TimeKeeping.Web.Shared.HttpModel;

namespace TimeKeeping.Web.Client
{
    public class AppStore
    {
        private Task _initTask;
        private Queue<StechzeitCommand> _queue = new();
        private readonly TimeKeepingClient _client;

        public AppStore(TimeKeepingClient client)
        {
            _client = client;
            _initTask = InitializeAsync();
        }

        public Task InitializeAsync()
        {
            if (_initTask != null)
                return _initTask;

            _queue = new();//TODO load from storage
            _ = RunSync();

            return Task.CompletedTask;
        }

        private async Task RunSync()
        {
            if (_queue.Any())
            {
                try
                {
                    while (_queue.Any() )
                    {
                        var command = _queue.Peek();
                        if (command.Action == StechzeitAction.Upsert)
                        {
                            var zeitstempelRequest = new ZeitstempelRequest
                            {
                                Datum = DateOnly.FromDateTime(command.Entry.At.ToLocalTime().Date),
                                Stempeltyp = command.Entry.Type,
                                Zeitstempel = command.Entry.At,
                            };
                            await _client.ErfasseZeitstempelAsync(command.ArbeitsplatzUuid, zeitstempelRequest);
                        }
                        else if (command.Action == StechzeitAction.Delete)
                        {
                            //TODO
                        }

                        //TODO persist queue
                        _queue.Dequeue();
                    }
                }
                catch
                {

                }
            }

            _ = Task.Delay(1000 * 60 * 15).ContinueWith(_ => RunSync());
        }

        public async Task Upsert(Guid arbeitsplatzUuid, Entry entry)
        {
            await _initTask;

            if (_queue.Any())
            {
                _queue.Enqueue(new StechzeitCommand { ArbeitsplatzUuid = arbeitsplatzUuid, Action = StechzeitAction.Upsert, Entry = entry });
                //TODO persist queue
            }
            else
            {
                try
                {
                    var zeitstempelRequest = new ZeitstempelRequest
                    {
                        Datum = DateOnly.FromDateTime(entry.At.ToLocalTime().Date),
                        Stempeltyp = entry.Type,
                        Zeitstempel = entry.At,
                    };
                    await _client.ErfasseZeitstempelAsync(arbeitsplatzUuid, zeitstempelRequest);
                }
                catch
                {
                    _queue.Enqueue(new StechzeitCommand { ArbeitsplatzUuid = arbeitsplatzUuid, Action = StechzeitAction.Upsert, Entry = entry });
                    //TODO persist queue
                }
            }
        }

        public async Task RemoveEntry(Entry entry)
        {
            await _initTask;
            await Task.Delay(11);
        }
    }

    //TODO: Erlauben wir andere offline Aktionen?
    internal class StechzeitCommand
    {
        public required Guid ArbeitsplatzUuid { get; set; }
        public required StechzeitAction Action { get; set; }
        public required Entry Entry { get; set; }
    }

    internal enum StechzeitAction
    {
        Upsert,
        Delete
    }
}
