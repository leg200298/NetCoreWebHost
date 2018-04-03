using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace WebHostConsole
{

    [ApiExplorerSettings(GroupName = "v1")]
    [Route("TodoAPI/Todo/[action]")]
    public class TodoController : Controller
    {
        //private readonly TodoContext _context;

        public TodoController()
        {

        }
        /// <summary>
        /// 測試
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetAll")]
        public IActionResult GetAll()
        {
            var test = new List<string>();
            test.Add("1");
            test.Add("2");
            test.Add("3");
            test.Add("4");
            return Ok(test);
        }
    }
}
