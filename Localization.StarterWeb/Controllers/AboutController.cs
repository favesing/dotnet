using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Localization.StarterWeb.Controllers
{
    public class AboutController : Controller
    {
        private readonly IStringLocalizer<AboutController> _localizer;
        public AboutController(IStringLocalizer<AboutController> localizer)
        {
            _localizer = localizer;
        }
        // GET: /<controller>/
        public string Index()
        {
            return _localizer["About Title"];
        }
    }
}
