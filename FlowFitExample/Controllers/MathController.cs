using FlowFitExample.Managers;
using Microsoft.AspNetCore.Mvc;

namespace FlowFitExample.Controllers
{
    [ApiController]
    [Route("/api/v1/math")]
    public class MathController : Controller
    {
        private readonly IMathManager _mathManager;

        public MathController(IMathManager mathManager)
        {
            _mathManager = mathManager;
        }

        public ActionResult<int> Add(int a, int b)
        {
            var result = _mathManager.Add(a, b);
            return Ok(result);
        }

        public ActionResult<double> SquareRoot(double a)
        {
            return Ok(_mathManager.SquareRoot(a));
        }

        public ActionResult<long> Factorial(int a)
        {
            return _mathManager.Factorial(a);
        }
    }
}
