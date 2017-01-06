using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Net.Http.Headers;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Error.DeveloperExceptionPage.Controllers
{
    public class ErrorController : Controller
    {
        private IHostingEnvironment Env;
        public ErrorController(IHostingEnvironment env)
        {
            Env = env;
        }
        // GET: /<controller>/
        [Route("error/{statusCode?}")]
        public IActionResult Index(int? statusCode)
        {
            //return "Error-Controller: Exception" + statusCode ?? "";

            var filePath = $"{Env.WebRootPath}/errors/{(statusCode == 404 ? 404 : 500)}.html";
            var fileResult = new PhysicalFileResult(filePath, new MediaTypeHeaderValue("text/html"));
            return fileResult;
        }
    }
}
