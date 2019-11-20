using System;
using System.Collections.Generic;
using FlowFitExample.Managers;
using FlowFitExample.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FlowFitExample.Controllers
{
    [ApiController]
    [Route("science")]
    public class ScienceController : Controller
    {
        private readonly IScienceManager _scienceManager;
        private readonly ILogger<ScienceController> _log;

        public ScienceController(ILogger<ScienceController> log, IScienceManager scienceManager)
        {
            _log = log;
            _scienceManager = scienceManager;
        }

        [HttpPut("dinosaurs/{hash}")]
        public ActionResult<DinosaurGenome> UpsertDinosaurGenome([FromQuery] string name, string hash)
        {
            var geneticData = new Span<byte>();
            if (!Convert.TryFromBase64String(hash, geneticData, out var bytesWritten))
            {
                return BadRequest("Invalid hash format for genetic data");
            }

            var dinosaur = _scienceManager.RegisterNewDinosaurGenome(name, geneticData.ToArray());
            return Ok(dinosaur);
        }

        [HttpDelete("dinosaurs/{hash}")]
        public ActionResult<bool> DeactivateDinosaurGenome(string hash)
        {
            try
            {
                _scienceManager.DeactivateDinosaurGenome(hash);
                return Ok(true);
            }
            catch (InvalidOperationException)
            {
                return Ok(false);
            }
        }

        [HttpGet("words")]
        public ActionResult<List<StringSegment>> GetAllScienceWords([FromQuery] int limit)
        {
            return Ok(_scienceManager.GetScienceyWords(limit));
        }
    }
}
