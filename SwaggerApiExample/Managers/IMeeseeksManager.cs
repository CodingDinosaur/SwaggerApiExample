using System;
using SwaggerApiExample.Models;
using System.Collections.Generic;
using SwaggerApiExample.Models.Meeseeks;

namespace SwaggerApiExample.Managers
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
