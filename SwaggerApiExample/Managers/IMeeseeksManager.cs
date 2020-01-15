using System;
using SwaggerApiExample.Models;
using System.Collections.Generic;
using SwaggerApiExample.Models.Meeseeks;

namespace SwaggerApiExample.Managers
{
    public interface IMeeseeksManager
    {
        MrMeeseeks SpawnMeeseeksForTask(BaseMeeseeksTask task);
        IEnumerable<BaseMeeseeksTask> GetAllRunningTasks();
        IEnumerable<MrMeeseeks> FindLateMeeseeks();
        IEnumerable<MrMeeseeks> GetAllMeeseeksOnTask(MeeseeksTaskCategory categoryFilter = MeeseeksTaskCategory.Unknown);
        MrMeeseeks GetMeeseeksById(Guid id);
    }
}
