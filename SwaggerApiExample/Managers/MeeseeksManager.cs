using System;
using System.Collections.Generic;
using System.Linq;
using SwaggerApiExample.Models;
using SwaggerApiExample.Models.Meeseeks;
using Microsoft.Extensions.Logging;

namespace SwaggerApiExample.Managers
{
    public class MeeseeksManager : IMeeseeksManager
    {
        private readonly ILogger _log;
        private readonly List<MrMeeseeks> _allMeeseeks = new List<MrMeeseeks>();

        public MeeseeksManager(ILogger<MeeseeksManager> log)
        {
            _log = log;
        }

        public MrMeeseeks SpawnMeeseeksForTask(BaseMeeseeksTask task)
        {
            var m = new MrMeeseeks(0, task);
            _allMeeseeks.Add(m);
            return m;
        }

        public IEnumerable<BaseMeeseeksTask> GetAllRunningTasks()
        {
            return _allMeeseeks.Select(m => m.CurrentTask);
        }

        public IEnumerable<MrMeeseeks> FindLateMeeseeks()
        {
            return _allMeeseeks.Where(m => m.IsLosingSanity);
        }

        public IEnumerable<MrMeeseeks> GetAllMeeseeksOnTask(MeeseeksTaskCategory taskCategoryFilter = MeeseeksTaskCategory.Unknown)
        {
            return _allMeeseeks.Where(m => taskCategoryFilter == MeeseeksTaskCategory.Unknown || m.CurrentTask.TaskCategory == taskCategoryFilter);
        }

        public MrMeeseeks GetMeeseeksById(Guid id)
        {
            return _allMeeseeks.FirstOrDefault(m => m.Id == id);
        }
    }
}
