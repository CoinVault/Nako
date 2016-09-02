using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nako.Api;

namespace coretest.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { DateTime.UtcNow.ToString() };
        }

        [Route("[action]")]
        public string Heartbeat()
        {
            return "Heartbeat";
        }

        [Route("[action]")]
        public async Task<IActionResult> Connections()
        {

            var response = this.CreateOkResponse(new Nako.Api.Handlers.Types.StatsConnection { Connections = 444444 });

            return await Task.FromResult(response);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
