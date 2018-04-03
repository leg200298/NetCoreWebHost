using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace WebHostConsole
{
    [ApiExplorerSettings(GroupName = "v2")]
    [Route("TestAPI/Test/[action]")]
    public class TestController : Controller
    {
        public TestController()
        {

        }
        [HttpGet(Name = "GetTestAll")]
        public IActionResult GetAll()
        {
            var test = new List<string>();
            test.Add("2");
            test.Add("3");
            test.Add("4");
            test.Add("5");
            return Ok(test);
        }
    }
}