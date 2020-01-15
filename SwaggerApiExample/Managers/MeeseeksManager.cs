using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SwaggerApiExample.Models.Meeseeks;
using Microsoft.Extensions.Logging;

namespace SwaggerApiExample.Managers
{
    public class MeeseeksManager : IMeeseeksManager
    {
        private readonly ILogger _log;
        private readonly List<MrMeeseeks> _allMeeseeks = new List<MrMeeseeks>();
        private readonly List<IMeeseeksTask> _allTasks = new List<IMeeseeksTask>();

        public MeeseeksManager(ILogger<MeeseeksManager> log)
        {
            _log = log;
        }

        public async Task<MrMeeseeks> SpawnMeeseeksForTaskAsync(IMeeseeksTask task)
        {
            var m = new MrMeeseeks(0);
            _log.LogInformation($"Spawning MrMeeseeks {m.Id} for a {task.TaskCategory} task");

            _allMeeseeks.Add(m);
            _allTasks.Add(task);

            _log.LogInformation($"Starting executing of a {task.TaskCategory} task");
            await task.ExecuteAsync(m);
            _log.LogDebug($"Task ExecuteAsync returned for Meeseeks {m.Id}");

            return m;
        }

        public IEnumerable<IMeeseeksTask> GetAllRunningTasks()
        {
            return _allTasks.Where(t => t.HasStarted);
        }

        public IEnumerable<MrMeeseeks> FindLateMeeseeks()
        {
            return _allMeeseeks.Where(m => m.IsLosingSanity);
        }

        public IEnumerable<MrMeeseeks> GetAllMeeseeksOnTask(MeeseeksTaskCategory taskCategoryFilter = MeeseeksTaskCategory.Unknown)
        {
            return _allTasks.Where(t => taskCategoryFilter == MeeseeksTaskCategory.Unknown || t.TaskCategory == taskCategoryFilter)
                .Join(_allMeeseeks,
                    task => task.AssignedMeeseeks,
                    meeseeks => meeseeks.Id,
                    (task, meeseeks) => meeseeks);
        }

        public MrMeeseeks GetMeeseeksById(Guid id)
        {
            return _allMeeseeks.FirstOrDefault(m => m.Id == id);
        }
    }
}
