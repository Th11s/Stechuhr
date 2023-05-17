using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using TimeKeeping.Web.Shared.HttpModel;

namespace TimeKeeping.Web.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntryController : ControllerBase
    {
        private readonly IMemoryCache _cache;

        public EntryController(IMemoryCache cache)
        {
            _cache = cache;
        }

        [HttpPost("upsert")]
        public IActionResult UpsertEntry(Entry entry)
        {
            var items = _cache.Get<List<Entry>>("items") ?? new List<Entry>();
            items = items.Where(x => x.Uuid != entry.Uuid).Append(entry).ToList();

            _cache.Set("items", items);

            return Ok();
        }

        [HttpDelete("{uuid}")]
        public IActionResult DeleteEntry(Guid uuid)
        {
            var items = _cache.Get<List<Entry>>("items") ?? new List<Entry>();
            items = items.Where(x => x.Uuid != uuid).ToList();

            _cache.Set("items", items);

            return Ok();
        }

        [HttpGet("/date/{date}")]
        public ActionResult<List<Entry>> GetBydate(DateTimeOffset date)
        {
            return _cache.Get<List<Entry>>("items") ?? new();
        }
    }
}
