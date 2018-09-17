using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using POC.Api.Model;

namespace POC.Api.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ValuesController : BaseController
    {
        #region private members

        #endregion

        public ValuesController(IMapper mapper) :base(mapper)
        {
        }

        // GET api/values
        [HttpGet]
        public ActionResult Get()
        {
            var city = new City { Name = "Toronto"};
            var county = _mapper.Map<County>(city);

            return Ok(county);
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
