using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Config.UsingOptions.Controllers
{
    public class HomeController : Controller
    {
        private readonly MyOptions _options;
        private readonly MySubOptions _subOptions;
        public HomeController(IOptions<MyOptions> options, IOptions<MySubOptions> subOptions)
        {
            this._options = options.Value;
            this._subOptions = subOptions.Value;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            var option1 = _options.Option1;
            var option2 = _options.Option2;

            var subOption1 = _subOptions.SubOption1;
            var subOption2 = _subOptions.SubOption2;
            return Content($"option1 = {option1}, options2 = {option2}, subOption1 = { subOption1 }, subOption2 = { subOption2 }");
        }
    }
}
