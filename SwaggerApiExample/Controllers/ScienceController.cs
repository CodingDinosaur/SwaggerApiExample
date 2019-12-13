using System;
using System.Collections.Generic;
using SwaggerApiExample.Managers;
using SwaggerApiExample.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace SwaggerApiExample.Controllers
{
    /// <summary>
    /// Science API - Perform random useless pseudo-scientific tasks!
    /// </summary>
    [ApiController]
    [Route("api/science")]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class ScienceController : Controller
    {
        private readonly IScienceManager _scienceManager;
        private readonly ILogger<ScienceController> _log;

        internal ScienceController(ILogger<ScienceController> log, IScienceManager scienceManager)
        {
            _log = log;
            _scienceManager = scienceManager;
        }

        /// <summary>
        /// Idempotent add / update Dinosaur genome
        /// </summary>
        /// <param name="name">Dinosaur common name</param>
        /// <param name="hash">Base64 string representation of genome hash</param>
        /// <returns></returns>
        [HttpPut("dinosaurs/{hash}")]
        public ActionResult<DinosaurGenome> UpsertDinosaurGenome([FromQuery] string name, string hash)
        {
            var geneticData = new Span<byte>();
            if (!Convert.TryFromBase64String(hash, geneticData, out _))
            {
                return BadRequest("Invalid hash format for genetic data");
            }

            var dinosaur = _scienceManager.RegisterNewDinosaurGenome(name, geneticData);
            return Ok(dinosaur);
        }

        /// <summary>
        /// Deactivate an existing Dinosaur genome
        /// </summary>
        /// <param name="hash">Base64 string representation of genome hash</param>
        /// <returns>True if deleted, false if no matching genome found to delete</returns>
        [HttpDelete("dinosaurs/{hash}")]
        [Produces("text/plain")]
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

        /// <summary>
        /// Get a list of non-sensical words that sound remotely sciencey
        /// </summary>
        /// <param name="limit">Maximum number of words to get</param>
        /// <returns>Nothing useful, honestly</returns>
        [HttpGet("words")]
        public ActionResult<List<StringSegment>> GetAllScienceWords([FromQuery] int limit)
        {
            return Ok(_scienceManager.GetScienceyWords(limit));
        }
    }
}
