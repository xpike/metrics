using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XPike.Metrics;
using XPike.Metrics.Microsoft;

namespace XPikeMetrics22.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : MetricsControllerBase
    {
        private readonly IMetricsService _metricsService;
        private readonly IMetricsContextAccessor _contextAccessor;

        public ValuesController(IMetricsService metricsService, IMetricsContextAccessor contextAccessor)
            : base(metricsService, contextAccessor)
        {
            _metricsService = metricsService;
            _contextAccessor = contextAccessor;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            using (var tracker = _metricsService.StartTracker("endpoint.request",
                tags: new[] {$"controller:{GetType().Name}", $"endpoint:{nameof(Get)}"}))
            {
                tracker.SetSuccess();
                return new string[] {"value1", "value2"};
            }
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public Task<ActionResult<string>> Get(int id) =>
            WithTimingAsync(tracker => HandleGetAsync(id, tracker));

        private async Task<ActionResult<string>> HandleGetAsync(int id, IOperationTracker tracker)
        {
            if (tracker == null)
                tracker = _contextAccessor.OperationTracker ?? tracker;

            _contextAccessor.MetricsContext.AddTag("test", "tag");
            tracker.SetSuccess("testing");
            await Task.Delay(500);
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
