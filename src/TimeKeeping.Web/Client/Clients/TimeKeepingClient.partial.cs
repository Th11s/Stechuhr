using System.Text.Json;
using System.Text.Json.Serialization;

namespace TimeKeeping.Web.Client.Clients
{
    public partial class TimeKeepingClient
    {
        partial void UpdateJsonSerializerSettings(JsonSerializerOptions settings)
        {
            settings.Converters.Add(new JsonStringEnumConverter());
            settings.PropertyNameCaseInsensitive = true;
        }
    }
}
