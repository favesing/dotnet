using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Localization.StarterWeb.Controllers
{
    public class InfoController : Controller
    {
        private readonly IStringLocalizer<InfoController> _localizer;
        private readonly IStringLocalizer<SharedResource> _sharedLocalizer;

        public InfoController(IStringLocalizer<InfoController> localizer,
                       IStringLocalizer<SharedResource> sharedLocalizer)
        {
            _localizer = localizer;
            _sharedLocalizer = sharedLocalizer;
        }

        // GET: /<controller>/
        public string Index()
        {
            string msg = "Shared resx: " + _sharedLocalizer["Hello!"]
                        + " Info resx " + _localizer["Hello!"];
            return msg;
        }
    }
}
