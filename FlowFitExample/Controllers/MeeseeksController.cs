using System;
using System.Linq;
using FlowFitExample.Managers;
using FlowFitExample.Models;
using FlowFitExample.Models.Frontend;
using FlowFitExample.Models.Meeseeks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FlowFitExample.Controllers
{
    [ApiController]
    [Route("api/meeseeks")]
    public class MeeseeksController : Controller
    {
        private readonly IMeeseeksManager _meeseeksManager;
        private readonly ILogger<MeeseeksController> _log;

        public MeeseeksController(ILogger<MeeseeksController> log, IMeeseeksManager meeseeksManager)
        {
            _log = log;
            _meeseeksManager = meeseeksManager;
        }

        [HttpPost("")]
        public IActionResult StartTask(StartMeeseeksTaskRequest request)
        {
            var (task, typeInfo) = GenerateMeeseeksTask(request);
            var genericMethod = _meeseeksManager.GetType().GetMethod(nameof(_meeseeksManager.SpawnMeeseeksForTask))
                ?.MakeGenericMethod(typeInfo);
            var meeseeksInfo = genericMethod?.Invoke(_meeseeksManager, new object[] { task }) as MrMeeseeks;

            if (meeseeksInfo == null) { return Problem(); }

            meeseeksInfo.CurrentTask.Execute();
            return Ok(new StartMeeseeksTaskResponse(true, meeseeksInfo, typeInfo.Name));
        }

        [HttpGet("")]
        public IActionResult GetAllRunningTasks()
        {
            var allTasks = _meeseeksManager.GetAllRunningTasks().ToList();
            return Ok(allTasks);
        }

        [HttpGet("genericTasks")]
        public IActionResult GetGenericTaskReport()
        {
            var results = _meeseeksManager.GetAllMeeseeksOnTask<GeneralMeeseeksTask>()
                .Select(m => new MeeseeksTaskStatus(m.Id, m.CurrentTask.TaskType.ToString(), m.CurrentTask));

            return Ok(results);
        }

        [HttpGet("{id:guid}")]
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
            _ => (new GeneralMeeseeksTask(MeeseeksTaskType.Unknown, request.TaskTypeName, _log) as BaseMeeseeksTask, typeof(GeneralMeeseeksTask)),
        };

    }
}
