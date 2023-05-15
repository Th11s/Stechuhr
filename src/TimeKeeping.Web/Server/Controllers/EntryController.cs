using Microsoft.AspNetCore.Mvc;
using TimeKeeping.Web.Shared.HttpModel;

namespace TimeKeeping.Web.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EntryController : ControllerBase
    {
        private List<Entry> _testEntries = new();

        [HttpPost("upsert")]
        public IActionResult UpsertEntry(Entry entry)
        {
            _testEntries = _testEntries.Where(x => x.Uuid != entry.Uuid).ToList();
            _testEntries.Add(entry);

            return Ok();
        }

        [HttpDelete("{uuid}")]
        public IActionResult DeleteEntry(Guid uuid)
        {
            _testEntries = _testEntries.Where(x => x.Uuid != uuid).ToList();

            return Ok();
        }
    }
}
