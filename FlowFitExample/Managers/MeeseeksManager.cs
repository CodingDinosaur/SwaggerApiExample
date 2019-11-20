using System;
using System.Collections.Generic;
using System.Linq;
using FlowFitExample.Models;
using Microsoft.Extensions.Logging;

namespace FlowFitExample.Managers
{
    public class MeeseeksManager : IMeeseeksManager
    {
        private readonly ILogger _log;
        private readonly List<MrMeeseeks> _allMeeseeks = new List<MrMeeseeks>();

        public MeeseeksManager(ILogger<MeeseeksManager> log)
        {
            _log = log;
        }

        public MrMeeseeks<TTask> SpawnMeeseeksForTask<TTask>(TTask task) where TTask : BaseMeeseeksTask
        {
            var m = new MrMeeseeks<TTask>(0, task);
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

        public IEnumerable<MrMeeseeks<TTask>> GetAllMeeseeksOnTask<TTask>() where TTask : BaseMeeseeksTask
        {
            return _allMeeseeks.Where(m => m.CurrentTask is TTask).Cast<MrMeeseeks<TTask>>();
        }

        public MrMeeseeks GetMeeseeksById(Guid id)
        {
            return _allMeeseeks.FirstOrDefault(m => m.Id == id);
        }
    }
}
