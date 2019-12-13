using System;
using System.Linq;
using SwaggerApiExample.Managers;
using SwaggerApiExample.Models;
using SwaggerApiExample.Models.Frontend;
using SwaggerApiExample.Models.Meeseeks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace SwaggerApiExample.Controllers
{
    /// <summary>
    /// Meeseeks API - Start and get information on Meeseeks Tasks
    /// </summary>
    [ApiController]
    [Route("api/meeseeks")]
    [Consumes("application/json")]
    [Produces("application/json", "text/html")]
    public class MeeseeksController : Controller
    {
        private readonly IMeeseeksManager _meeseeksManager;
        private readonly ILogger<MeeseeksController> _log;

        internal MeeseeksController(ILogger<MeeseeksController> log, IMeeseeksManager meeseeksManager)
        {
            _log = log;
            _meeseeksManager = meeseeksManager;
        }

        /// <summary>
        /// Kick off a new task.  Spawns a new Mr. Meeseeks
        /// </summary>
        /// <remarks>
        /// Kicks off a new tasks and spawns a new Mr. Meeseeks to complete that task.  Mr. Meeseeks will stop at nothing to ensure the task is completed.
        /// It is strongly advised not to assign long-running tasks to a Mr. Meeseeks, as their lifespans typically do not exceed minutes or hours, there is
        /// a strong diminishing return of mental capacity after around 24 hours.  By the 48 hour mark, a Mr. Meeseeks may become delerious or hostile.
        /// <br/>Eventually, if the task continues to go uncompleted, the Meeseeks may resort to extreme measures to attempt to fulfill his duty or negate his need, including killing the task initiator.
        /// </remarks>
        /// <param name="request">Meeseeks request information</param>
        /// <returns></returns>
        [HttpPost("")]
        [ProducesResponseType(typeof(StartMeeseeksTaskResponse), 200)]
        [ProducesResponseType(typeof(TaskStartFailureInfo), 500)]
        public IActionResult StartTask(StartMeeseeksTaskRequest request)
        {
            var (task, typeInfo) = GenerateMeeseeksTask(request);
            if (task.TaskCategory == MeeseeksTaskCategory.Unknown)
            {
                return StatusCode(500, new TaskStartFailureInfo("Task generator was unable to determine task type"));
            }

            var genericMethod = _meeseeksManager.GetType().GetMethod(nameof(_meeseeksManager.SpawnMeeseeksForTask))
                ?.MakeGenericMethod(typeInfo);
            var meeseeksInfo = genericMethod?.Invoke(_meeseeksManager, new object[] { task }) as MrMeeseeks;
            
            if (meeseeksInfo == null) { return Problem(); }

            meeseeksInfo.CurrentTask.Execute();
            return Ok(new StartMeeseeksTaskResponse(meeseeksInfo, typeInfo.Name));
        }

        /// <summary>
        /// Get basic information for all running Meeseeks tasks
        /// </summary>
        /// <returns></returns>
        [HttpGet("")]
        [Produces(typeof(BaseMeeseeksTask[]))]
        [ProducesResponseType(typeof(BaseMeeseeksTask[]), 200)]
        public IActionResult GetAllRunningTasks()
        {
            var allTasks = _meeseeksManager.GetAllRunningTasks().ToArray();
            return Ok(allTasks);
        }

        /// <summary>
        /// Get a detailed report on all Generic-type Meeseeks tasks, including info for the associated Mr. Meeseeks
        /// </summary>
        /// <returns></returns>
        [HttpGet("genericTasks")]
        [Produces(typeof(MeeseeksTaskStatus[]))]
        public IActionResult GetGenericTaskReport()
        {
            var results = _meeseeksManager.GetAllMeeseeksOnTask<GeneralMeeseeksTask>()
                .Select(m => new MeeseeksTaskStatus(m.Id, m.CurrentTask.TaskCategory, m.CurrentTask))
                .ToArray();

            return Ok(results);
        }

        /// <summary>
        /// Get details for a specific Mr. Meeseeks instance by ID
        /// </summary>
        /// <param name="id">GUID of the requested Mr. Meeseeks</param>
        /// <remarks>See <see cref="MeeseeksTaskCategory"/> for valid values</remarks>
        [HttpGet("{id:guid}")]
        [Produces(typeof(MrMeeseeks))]
        public IActionResult GetMeeseeksById([FromRoute] Guid id)
        {
            var mrMeeseeks = _meeseeksManager.GetMeeseeksById(id);
            if (mrMeeseeks == null)
            {
                return NotFound($"No Mr. Meeseeks Found with ID: {id:D}");
            }

            return Ok(mrMeeseeks);
        }

        private (BaseMeeseeksTask Task, Type TypeInfo) GenerateMeeseeksTask(StartMeeseeksTaskRequest request)
            => request switch
        {
            { TaskTypeName: nameof(ImproveGolfMeeseeksTask) } => (new ImproveGolfMeeseeksTask(_log) as BaseMeeseeksTask, typeof(ImproveGolfMeeseeksTask)),
            { TaskTypeName: nameof(ImprovePinballMeeseeksTask) } => (new ImprovePinballMeeseeksTask(_log) as BaseMeeseeksTask, typeof(ImprovePinballMeeseeksTask)),
            _ => (new GeneralMeeseeksTask(MeeseeksTaskCategory.Unknown, request.TaskTypeName, _log) as BaseMeeseeksTask, typeof(GeneralMeeseeksTask)),
        };

    }
}
