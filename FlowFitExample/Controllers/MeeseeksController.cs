using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FlowFitExample.Controllers
{
    [ApiController]
    [Route("meeseeks")]
    public class MeeseeksController : Controller
    {
        private ILogger<MeeseeksController> _log;

        public MeeseeksController(ILogger<MeeseeksController> log)
        {
            _log = log;
        }
    }
}
