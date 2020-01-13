using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using XPike.Metrics;

namespace XPikeMetrics22.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IMetricsService _metricsService;

        public ValuesController(IMetricsService metricsService)
        {
            _metricsService = metricsService;
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
        public ActionResult<string> Get(int id)
        {
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
