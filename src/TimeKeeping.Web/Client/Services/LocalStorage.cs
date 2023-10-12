using Microsoft.JSInterop;
using System.Text.Json;
using JGUZDV.Extensions.Json;
namespace TimeKeeping.Web.Client.Services;

public class LocalStorage : IKeyValueStorage
{
    private readonly IJSRuntime _JSRuntime;


    public LocalStorage(IJSRuntime jSRuntime)
    {
        _JSRuntime = jSRuntime;
    }

    public Task SetItem<T>(string key, T value, DateTimeOffset? expiresAt = null)
    {
        var options = new JsonSerializerOptions();
        options.SetJGUZDVDefaults();

        var entry = new CacheEntry<T>
        {
            ExpiresAt = expiresAt,
            Value = value
        };

        var json = JsonSerializer.Serialize(entry, options);

        return _JSRuntime.InvokeVoidAsync("localStorage.setItem", key, json).AsTask();
    }

    public async Task<T?> GetItem<T>(string key) where T : class
    {
        var value = await _JSRuntime.InvokeAsync<string>("localStorage.getItem", key);

        if (value == null)
            return null;

        var options = new JsonSerializerOptions();
        options.SetJGUZDVDefaults();

        try
        {
            var entry = JsonSerializer.Deserialize<CacheEntry<T>>(value, options);
            if (entry == null)
                return null;

            if (entry.ExpiresAt.HasValue && DateTimeOffset.Now > entry.ExpiresAt)
            {
                _ = RemoveItem(key);
                return null;
            }

            return entry.Value;
        }
        catch
        {
            _ = RemoveItem(key);
            return null;
        }

    }
    public async Task RemoveItem(string key)
    {
        await _JSRuntime.InvokeVoidAsync("localStorage.removeItem", key);
    }

    public async Task Clear()
    {
        await _JSRuntime.InvokeVoidAsync("localStorage.clear");
    }

    private class CacheEntry<T>
    {
        public required T Value { get; set; }
        public DateTimeOffset? ExpiresAt { get; set; }
    }
}
