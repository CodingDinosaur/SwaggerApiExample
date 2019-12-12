using System;
using FlowFitExample.Models;
using System.Collections.Generic;
using FlowFitExample.Models.Meeseeks;

namespace FlowFitExample.Managers
{
    public interface IMeeseeksManager
    {
        MrMeeseeks<TTask> SpawnMeeseeksForTask<TTask>(TTask task) where TTask : BaseMeeseeksTask;
        IEnumerable<BaseMeeseeksTask> GetAllRunningTasks();
        IEnumerable<MrMeeseeks> FindLateMeeseeks();
        IEnumerable<MrMeeseeks<TTask>> GetAllMeeseeksOnTask<TTask>() where TTask : BaseMeeseeksTask;
        MrMeeseeks GetMeeseeksById(Guid id);
    }
}
