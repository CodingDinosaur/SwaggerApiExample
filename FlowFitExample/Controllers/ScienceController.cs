using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FlowFitExample.Controllers
{
    [ApiController]
    [Route("science")]
    public class ScienceController : Controller
    {
        private ILogger<ScienceController> _log;

        public ScienceController(ILogger<ScienceController> log)
        {
            _log = log;
        }
    }
}
