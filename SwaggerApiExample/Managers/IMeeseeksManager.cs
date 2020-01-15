using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SwaggerApiExample.Models.Meeseeks;

namespace SwaggerApiExample.Managers
{
    public interface IMeeseeksManager
    {
        Task<MrMeeseeks> SpawnMeeseeksForTaskAsync(IMeeseeksTask task);
        IEnumerable<IMeeseeksTask> GetAllRunningTasks();
        IEnumerable<MrMeeseeks> FindLateMeeseeks();
        IEnumerable<MrMeeseeks> GetAllMeeseeksOnTask(MeeseeksTaskCategory categoryFilter = MeeseeksTaskCategory.Unknown);
        MrMeeseeks GetMeeseeksById(Guid id);
    }
}
