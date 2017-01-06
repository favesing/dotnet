using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Localization.StarterWeb.Controllers
{
    public class TestController : Controller
    {
        private readonly IStringLocalizer _localizer;
        private readonly IStringLocalizer _localizer2;

        public TestController(IStringLocalizerFactory factory)
        {
            _localizer = factory.Create(typeof(SharedResource));
            _localizer2 = factory.Create("SharedResource", location: null);
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            ViewData["Message"] = 
                _localizer["Your application description page."]
                + " loc 2: " 
                + _localizer2["Your application description page."];

            return View();
        }
    }
}

namespace Localization.StarterWeb
{
    public class SharedResource
    {
    }
}
