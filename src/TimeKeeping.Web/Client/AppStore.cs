using Th11s.TimeKeeping.SharedModel.Requests;
using TimeKeeping.Web.Client.Clients;
using TimeKeeping.Web.Client.Services;
using TimeKeeping.Web.Shared.HttpModel;

namespace TimeKeeping.Web.Client
{
    public class AppStore
    {
        public enum SyncResult
        {
            Error,
            Success,
            Cancelled
        }

        public event Action<SyncResult>? SyncCompleted;

        private Task _initTask;
        private Queue<StechzeitCommand> _queue = new();
        private readonly TimeKeepingClient _client;
        private readonly IKeyValueStorage _storage;

        private readonly string _storageKey = "appstore_queue";

        private CancellationTokenSource _jobCts = new();
        private Task _jobTask = Task.CompletedTask;

        public AppStore(TimeKeepingClient client, IKeyValueStorage storage)
        {
            _client = client;
            _initTask = InitializeAsync();
            _storage = storage;
        }

        public async Task Upsert(Guid arbeitsplatzUuid, Entry entry)
        {
            await _initTask;

            _jobCts.Cancel();
            await _jobTask;

            _queue.Enqueue(new StechzeitCommand { ArbeitsplatzUuid = arbeitsplatzUuid, Action = StechzeitAction.Upsert, Entry = entry });
            await _storage.SetItem(_storageKey, _queue);

            _jobCts = new();
            _jobTask = RunSync(_jobCts.Token);
        }

        public async Task RemoveEntry(Entry entry)
        {
            await _initTask;

            _jobCts.Cancel();
            await _jobTask;

            _queue.Enqueue(new StechzeitCommand
            {
                ArbeitsplatzUuid = default,
                Action = StechzeitAction.Delete,
                Entry = entry
            });
            await _storage.SetItem(_storageKey, _queue);

            _jobCts = new();
            _jobTask = RunSync(_jobCts.Token);
        }

        private async Task InitializeAsync()
        {
            if (_initTask != null)
                await _initTask;

            _queue = await _storage.GetItem<Queue<StechzeitCommand>>(_storageKey) ?? new();
            _jobTask = RunSync(_jobCts.Token);
        }

        private async Task RunSync(CancellationToken ct)
        {
            if (_queue.Any())
            {
                try
                {
                    while (_queue.Any())
                    {
                        if (ct.IsCancellationRequested)
                        {
                            SyncCompleted?.Invoke(SyncResult.Cancelled);
                            return;
                        }

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
                            await _client.EntferneZeitstempelAsync(command.Entry.Uuid);
                        }

                        _queue.Dequeue();
                        await _storage.SetItem(_storageKey, _queue);
                    }

                    SyncCompleted?.Invoke(SyncResult.Success);
                }
                catch
                {
                    SyncCompleted?.Invoke(SyncResult.Error);
                }
            }

            _jobCts.Cancel();
            _jobCts = new();
            _ = Task.Delay(1000 * 60 * 15, _jobCts.Token)
                .ContinueWith(_ => _jobTask = RunSync(_jobCts.Token), TaskContinuationOptions.OnlyOnRanToCompletion);
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
