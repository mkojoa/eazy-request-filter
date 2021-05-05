using eazy.request.filter.Filter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestRequestfilter.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestRequestController : ControllerBase
    {
        public TestRequestController()
        {

        }

        [HttpGet("{CompanyId}")]
        [ValidateCompanyOnRequest]
        public async Task<IActionResult> GetSomething(Guid CompanyId)
        {
            return Ok(CompanyId);
        }
    }
}
