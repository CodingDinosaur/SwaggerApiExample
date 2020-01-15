using System;
using System.Linq;
using System.Threading.Tasks;
using SwaggerApiExample.Managers;
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

        public MeeseeksController(ILogger<MeeseeksController> log, IMeeseeksManager meeseeksManager)
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
        [HttpPost("tasks")]
        [ProducesResponseType(typeof(StartMeeseeksTaskResponse), 200)]
        [ProducesResponseType(typeof(TaskStartFailureInfo), 500)]
        public async Task<IActionResult> StartTask(StartMeeseeksTaskRequest request)
        {
            var (task, typeInfo) = GenerateMeeseeksTask(request);
            if (task.TaskCategory == MeeseeksTaskCategory.Unknown)
            {
                return StatusCode(500, new TaskStartFailureInfo("Task generator was unable to determine task type"));
            }

            var meeseeksInfo = await _meeseeksManager.SpawnMeeseeksForTaskAsync(task);
            return meeseeksInfo == null 
                ? Problem() 
                : Ok(new StartMeeseeksTaskResponse(meeseeksInfo, typeInfo.Name));
        }

        /// <summary>
        /// Get basic information for all running Meeseeks tasks
        /// </summary>
        /// <returns></returns>
        [HttpGet("tasks")]
        [ProducesResponseType(typeof(MeeseeksTaskStatus[]), 200)]
        [ProducesResponseType(typeof(FailureInfo), 418)]
        public IActionResult GetAllActiveTasks()
        {
            var allRunningTasks = _meeseeksManager.GetAllRunningTasks()
                .Select(t => new MeeseeksTaskStatus(t));
            return allRunningTasks.Any()
                ? Ok(allRunningTasks)
                : StatusCode(418, new FailureInfo("No running tasks, so apparently I'm a teapot"));
        }

        /// <summary>
        /// Get a detailed report on all Meeseeks running tasks, including info for the associated Mr. Meeseeks
        /// </summary>
        /// <returns></returns>
        [HttpGet("")]
        [Produces(typeof(MeeseeksTaskStatusDetailed[]))]
        public IActionResult GetMeeseeksTaskReport([FromQuery] MeeseeksTaskCategory taskCategoryFilter = MeeseeksTaskCategory.Unknown)
        {
            var allTasks = _meeseeksManager.GetAllRunningTasks();
            var results = _meeseeksManager.GetAllMeeseeksOnTask(taskCategoryFilter)
                .Join(allTasks,
                    mrMeeseeks => mrMeeseeks.Id,
                    task => task.AssignedMeeseeks,
                    (mrMeeseeks, task) => new MeeseeksTaskStatusDetailed(task, mrMeeseeks))
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

        private (IMeeseeksTask Task, Type TypeInfo) GenerateMeeseeksTask(StartMeeseeksTaskRequest request)
            => request switch
        {
            { TaskTypeName: nameof(ImproveGolfMeeseeksTask) } => (new ImproveGolfMeeseeksTask(_log) as IMeeseeksTask, typeof(ImproveGolfMeeseeksTask)),
            { TaskTypeName: nameof(ImprovePinballMeeseeksTask) } => (new ImprovePinballMeeseeksTask(_log) as IMeeseeksTask, typeof(ImprovePinballMeeseeksTask)),
            _ => (new GeneralMeeseeksTask(MeeseeksTaskCategory.Simple, _log, request?.TaskTypeName) as IMeeseeksTask, typeof(GeneralMeeseeksTask)),
        };

    }
}
